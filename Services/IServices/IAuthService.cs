using OnlineCourse.Dtos;
using OnlineCourse.Primitives;

namespace OnlineCourse.Services.IServices;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequest);
    Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest);
    //Task<Result<Success>> LogoutAsync(RefreshTokenRequestDto refreshTokenRequest); // Using DTO for consistency, though only RefreshToken string is needed.
}
