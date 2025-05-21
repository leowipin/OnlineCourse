using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OnlineCourse.Data.Constants;
using OnlineCourse.Dtos;
using OnlineCourse.Entities;
using OnlineCourse.Primitives;
using OnlineCourse.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace OnlineCourse.Services;

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration, ApplicationDbContext dbContext) : IAuthService
{
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user is null)
        {
            return Result<LoginResponseDto>.Failure(new InvalidCredentialsError());
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            return Result<LoginResponseDto>.Failure(new AccountLockedOutError());
        }

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            return Result<LoginResponseDto>.Failure(new EmailNotConfirmedError());
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
            {
                return Result<LoginResponseDto>.Failure(new AccountLockedOutError());
            }
            return Result<LoginResponseDto>.Failure(new InvalidCredentialsError());
        }

        var (jwtSecurityToken, tokenString) = await GenerateJwtToken(user);
        var refreshTokenValue = GenerateRefreshTokenString();
        int refreshTokenExpiryDays = configuration.GetSection("JwtSettings").GetValue("RefreshTokenExpiryDays", 7);

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshTokenValue,
            JwtId = jwtSecurityToken.Id, // JTI of the access token
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
            IsUsed = false,
        };

        await dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
        await dbContext.SaveChangesAsync();

        var loginResponse = new LoginResponseDto
        {
            Token = tokenString,
            Email = user.Email!,
            RefreshToken = refreshTokenValue
        };
        return Result<LoginResponseDto>.Success(loginResponse);
    }

    private async Task<(JwtSecurityToken, string)> GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("ss", user.SecurityStamp!), // Added Security Stamp claim
        };

        // Roles
        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Permisos del usuario
        var userClaims = await userManager.GetClaimsAsync(user);
        var permissionClaims = userClaims.Where(c => c.Type == AppClaimTypes.Permission).ToList();

        // Permisos de los roles
        var rolePermissionClaims = new List<Claim>();
        foreach (var roleName in roles)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                rolePermissionClaims.AddRange(roleClaims.Where(c => c.Type == AppClaimTypes.Permission));
            }
        }

        // Combinar y evitar duplicados
        var allPermissionClaims = permissionClaims
            .Concat(rolePermissionClaims)
            .GroupBy(c => c.Type + c.Value)
            .Select(g => g.First())
            .ToList();

        claims.AddRange(allPermissionClaims);

        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"] ?? "60"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            Expires = expires,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor) as JwtSecurityToken;
        // Ensure securityToken is not null and is of type JwtSecurityToken
        if (securityToken == null)
        {
            throw new InvalidOperationException("Failed to create JWT security token.");
        }
        return (securityToken, tokenHandler.WriteToken(securityToken));
    }

    private string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[64]; // Increased size for more entropy
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
