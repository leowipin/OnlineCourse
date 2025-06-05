namespace OnlineCourse.Dtos;

public record RefreshTokenRequestDto
{
    public required string RefreshToken { get; init; }
    public required string ExpiredAccesToken { get; init; }
}