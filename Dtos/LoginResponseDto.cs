namespace OnlineCourse.Dtos;

public record LoginResponseDto
{
    public required string Token { get; init; }
    public required string Email { get; init; }
    public required string RefreshToken { get; init; }
}