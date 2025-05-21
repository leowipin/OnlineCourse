using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Dtos;
using OnlineCourse.Services.IServices;
using OnlineCourse.Web;

namespace OnlineCourse.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous] // Login endpoint should be accessible without prior authentication
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequest)
    {
        var result = await authService.LoginAsync(loginRequest);

        return result.Match(
            loginResponse => Ok(loginResponse), // Directly return the LoginResponseDto
            error => this.HandleServiceError(error, logger, nameof(Login))
        );
    }

    [HttpPost("refresh")]
    [AllowAnonymous] // Refresh endpoint should be accessible without a valid *access* token
    public async Task<ActionResult<LoginResponseDto>> Refresh([FromBody] RefreshTokenRequestDto refreshTokenRequest)
    {
        var result = await authService.RefreshTokenAsync(refreshTokenRequest);

        return result.Match(
            loginResponse => Ok(loginResponse),
            error => this.HandleServiceError(error, logger, nameof(Refresh))
        );
    }

    [HttpPost("logout")]
    [AllowAnonymous] // Or [Authorize] if you want to ensure an active access token is used to authorize logout
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto refreshTokenRequest)
    {
        var result = await authService.LogoutAsync(refreshTokenRequest);

        return result.Match(
            _ => Ok(new { message = "Logged out successfully." }), // On success, return 200 OK
            error => this.HandleServiceError(error, logger, nameof(Logout))
        );
    }

    // Example of a protected endpoint to test the token
    // [HttpGet("me")]
    // [Authorize] // Requires a valid JWT
    // public IActionResult GetCurrentUser()
    // {
    //     var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    //     var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
    //     var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(r => r.Value).ToList();
    //     var permissions = User.FindAll(OnlineCourse.Data.Constants.AppClaimTypes.Permission).Select(p => p.Value).ToList();

    //     if (userId is null)
    //     {
    //         return Unauthorized(); // Should not happen if [Authorize] is working
    //     }

    //     return Ok(new 
    //     {
    //         UserId = userId,
    //         Email = email,
    //         Roles = roles,
    //         Permissions = permissions
    //     });
    // }
}
