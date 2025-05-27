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
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Extensions.Logging;
using OnlineCourse.Exceptions;

namespace OnlineCourse.Services;

public class AuthService(UserManager<User> userManager, 
    SignInManager<User> signInManager, 
    IConfiguration configuration, 
    ApplicationDbContext context,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<Result<LoginResponseDto>> LoginAsync(
        LoginRequestDto loginRequest,
        string? endpointInfo = null,
        CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        
        if (user is null)
        {
            var invalidCredentialserror = new InvalidCredentialsError(loginRequest.Email);
            logger.LogServiceEvent(invalidCredentialserror, LogLevel.Information, endpointInfo);
            return Result<LoginResponseDto>.Failure(invalidCredentialserror);
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            var accountLockedOutError = new AccountLockedOutError(user.Id);
            logger.LogServiceEvent(accountLockedOutError, LogLevel.Information, endpointInfo);
            return Result<LoginResponseDto>.Failure(accountLockedOutError);
        }

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            var emailNotConfirmedError = new EmailNotConfirmedError(user.Email!);
            logger.LogServiceEvent(emailNotConfirmedError, LogLevel.Information, endpointInfo);
            return Result<LoginResponseDto>.Failure(emailNotConfirmedError);
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            if (signInResult.IsLockedOut)
            {
                var accountLockedOutError = new AccountLockedOutError(user.Id);
                logger.LogServiceEvent(accountLockedOutError, LogLevel.Information, endpointInfo);
                return Result<LoginResponseDto>.Failure(accountLockedOutError);
            }
            var invalidCredentialError = new InvalidCredentialsError(user.Email!);
            logger.LogServiceEvent(invalidCredentialError, LogLevel.Information, endpointInfo);
            return Result<LoginResponseDto>.Failure(invalidCredentialError);
        }

        var (jwtSecurityToken, tokenString) = await GenerateJwtToken(user, ct );
        var refreshTokenValue = GenerateRefreshTokenString();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshTokenValue,
            JwtId = jwtSecurityToken.Id,
            UserId = user.Id,
        };

        await context.RefreshTokens.AddAsync(refreshTokenEntity, ct);
        await context.SaveChangesAsync(ct);

        var loginResponse = new LoginResponseDto
        {
            Token = tokenString,
            Email = user.Email!,
            RefreshToken = refreshTokenValue
        };
        return Result<LoginResponseDto>.Success(loginResponse);
    }

    private async Task<(JwtSecurityToken, string)> GenerateJwtToken(
        User user,
        CancellationToken ct = default)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(AppClaimTypes.SecurityStamp, user.SecurityStamp!),
        };

        // Roles
        var userRoleNames = await userManager.GetRolesAsync(user);
        claims.AddRange(userRoleNames.Select(userRoleName => new Claim(ClaimTypes.Role, userRoleName)));

        if (userRoleNames.Any())
        {
            throw new ApiException(
                errorCode: "AUTH_USER_MISSING_ROLES",
                errorTitle: "User Lacks Required Roles for Token Generation",
                message: $"The user with ID '{user.Id}' does not have any assigned roles. " +
                        "A user must have at least one role to generate a JWT.");
        }

        // Permisos del usuario
        var userPermissionClaims = (await userManager.GetClaimsAsync(user))
            .Where(c => c.Type == AppClaimTypes.Permission)
            .ToList();

        // Permisos de los roles
        var roleBasedPermissionClaims = new List<Claim>();

        var roleIds = await context.Roles
            .Where(r => userRoleNames.Contains(r.Name!))
            .Select(r => r.Id)
            .ToListAsync(ct);

        if (roleIds.Count == 0)
        {
            throw new ApiException(
                errorCode: "AUTH_USER_MISSING_ROLES",
                errorTitle: "User Lacks Required Roles for Token Generation",
                message: $"User ID '{user.Id}' has assigned roles ('{string.Join(", ", userRoleNames)}') " +
                         "that were not found in the database. Roles may have been deleted or modified.");
        }

        roleBasedPermissionClaims = await context.RoleClaims
            .Where(rc => roleIds.Contains(rc.RoleId) && rc.ClaimType == AppClaimTypes.Permission)
            .Select(rc => new Claim(rc.ClaimType!, rc.ClaimValue!))
            .ToListAsync(ct);

        // Combinar y evitar duplicados
        var allPermissionClaims = userPermissionClaims
            .Concat(roleBasedPermissionClaims)
            .GroupBy(c => c.Type + c.Value)
            .Select(g => g.First())
            .ToList();

        claims.AddRange(allPermissionClaims);

        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"] ?? "30"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            Expires = expires,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        if (tokenHandler.CreateToken(tokenDescriptor) is not JwtSecurityToken securityToken)
        {
            throw new ApiException(
                errorCode: "AUTH_TOKEN_GENERATION_FAILED",
                errorTitle: "JWT Token Generation Error",
                message: $"Failed to generate a JWT for user with ID '{user.Id}'. " +
                            "The JwtSecurityTokenHandler returned a null token object. " +
                            "This indicates an unexpected internal error during token creation.");
        }
        return (securityToken, tokenHandler.WriteToken(securityToken));
    }

    private static string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    public async Task<Result<LoginResponseDto>> RefreshTokenAsync(
        RefreshTokenRequestDto refreshTokenRequest,
        string? endpointInfo = null,
        CancellationToken ct = default)
    {
        // 1. Validate the incoming refresh token
        var existingRefreshToken = await context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == refreshTokenRequest.RefreshToken);

        if (existingRefreshToken is null)
        {
            return Result<LoginResponseDto>.Failure(new InvalidTokenError());
        }

        if (existingRefreshToken.ExpiryDate < DateTime.UtcNow)
        {
            return Result<LoginResponseDto>.Failure(new InvalidTokenError());
        }

        if (existingRefreshToken.IsUsed)
        {
            var userTokens = await context.RefreshTokens
                .Where(rt => rt.UserId == existingRefreshToken.UserId)
                .ExecuteDeleteAsync();

            var invalidTokenError = new InvalidTokenError();
            logger.LogServiceEvent(invalidTokenError, LogLevel.Warning);

            return Result<LoginResponseDto>.Failure(invalidTokenError);
        }

        var user = existingRefreshToken.User;
        if (user is null) // Should not happen if DB constraints are correct, but good to check
        {
            return Result<LoginResponseDto>.Failure(new InvalidTokenError());
        }

        // Optional: Security Stamp check against the user. 
        // While the SecurityStampValidationFilter protects endpoints, an extra check here ensures that 
        // if a security stamp changed *after* an access token expired but *before* this refresh attempt,
        // we don't issue new tokens. This is belt-and-suspenders.
        // This step might be complex if the original access token's JTI was used to find the user's original security stamp.
        // For now, we rely on the current user's stamp.

        // 2. Mark the used refresh token as IsUsed = true
        existingRefreshToken.IsUsed = true;
        // dbContext.RefreshTokens.Update(existingRefreshToken); // EF Core tracks changes, explicit Update often not needed

        // 3. Generate new access token
        var (newJwtSecurityToken, newAccessTokenString) = await GenerateJwtToken(user);

        // 4. Generate new refresh token
        var newRefreshTokenValue = GenerateRefreshTokenString();
        var refreshTokenExpiryMinutes = configuration.GetValue<double?>("JwtSettings:RefreshTokenExpiryMinutes") ?? 10080;

        var newRefreshTokenEntity = new RefreshToken // Assuming RefreshTokenEntity.cs
        {
            Token = newRefreshTokenValue,
            JwtId = newJwtSecurityToken.Id, // JTI of the new access token
            UserId = user.Id,
            //CreationDate = DateTime.UtcNow,
            //ExpiryDate = DateTime.UtcNow.AddMinutes(refreshTokenExpiryMinutes),
            //IsUsed = false,
        };

        await context.RefreshTokens.AddAsync(newRefreshTokenEntity);
        await context.SaveChangesAsync(); // This will save changes to existingRefreshToken (IsUsed) and add newRefreshTokenEntity

        // 5. Return new tokens
        var loginResponse = new LoginResponseDto
        {
            Token = newAccessTokenString,
            Email = user.Email!,
            RefreshToken = newRefreshTokenValue
        };

        return Result<LoginResponseDto>.Success(loginResponse);
    }
    //public async Task<Result<Success>> LogoutAsync(RefreshTokenRequestDto refreshTokenRequest)
    //{
    //    var refreshTokenValue = refreshTokenRequest.RefreshToken;
    //    var existingRefreshToken = await dbContext.RefreshTokens
    //        .FirstOrDefaultAsync(rt => rt.Token == refreshTokenValue);

    //    if (existingRefreshToken is null)
    //    {
    //        // Token not found, could be considered success or a specific error.
    //        // For now, let's treat it as an error if the client expects a token to be revoked.
    //        return Result<Success>.Failure(new InvalidTokenError("Refresh token not found."));
    //    }

    //    if (existingRefreshToken.IsRevoked)
    //    {
    //        // Already revoked, can be considered a success.
    //        return Result<Success>.Success(new Success());
    //    }

    //    if (existingRefreshToken.IsUsed)
    //    {
    //        // If it was used, it should have been replaced. Revoking a used token might indicate 
    //        // a state mismatch or an attempt to logout with an old, consumed token.
    //        // For simplicity, we can also treat this as a success (it's no longer valid for refresh anyway).
    //        return Result<Success>.Success(new Success());
    //    }

    //    existingRefreshToken.IsRevoked = true;
    //    // No need to set IsUsed = true here, as revoking is a terminal state for this token.

    //    await dbContext.SaveChangesAsync();

    //    return Result<Success>.Success(new Success());
    //}
}
