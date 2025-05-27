using OnlineCourse.Dtos;
using OnlineCourse.Primitives;

namespace OnlineCourse.Services.IServices;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> LoginAsync(
        LoginRequestDto loginRequest,
        string? endpointInfo = null,
        CancellationToken ct = default);
    Task<Result<LoginResponseDto>> RefreshTokenAsync(
        RefreshTokenRequestDto refreshTokenRequest,
        string? endpointInfo = null,
        CancellationToken ct = default);
    //Task<Result<Success>> LogoutAsync(RefreshTokenRequestDto refreshTokenRequest); // Using DTO for consistency, though only RefreshToken string is needed.
}
