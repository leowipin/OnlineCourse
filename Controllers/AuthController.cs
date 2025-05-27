using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Dtos;
using OnlineCourse.Services.IServices;
using OnlineCourse.Web;

namespace OnlineCourse.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto loginRequest,
        CancellationToken ct)
    {
        var result = await authService.LoginAsync(loginRequest, nameof(Login), ct);

        return result.Match(
            loginResponse => Ok(loginResponse),
            error => this.HandleServiceError(error)
        );
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<LoginResponseDto>> Refresh(
        [FromBody] RefreshTokenRequestDto refreshTokenRequest,
        CancellationToken ct)
    {
        var result = await authService.RefreshTokenAsync(refreshTokenRequest, nameof(Refresh), ct);

        return result.Match(
            loginResponse => Ok(loginResponse),
            error => this.HandleServiceError(error)
        );
    }

    //[HttpPost("logout")]
    //[AllowAnonymous] // Or [Authorize] if you want to ensure an active access token is used to authorize logout
    //public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto refreshTokenRequest)
    //{
    //    var result = await authService.LogoutAsync(refreshTokenRequest);

    //    return result.Match(
    //        _ => Ok(new { message = "Logged out successfully." }), // On success, return 200 OK
    //        error => this.HandleServiceError(error, logger, nameof(Logout))
    //    );
    //}

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
