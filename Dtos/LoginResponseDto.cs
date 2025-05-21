namespace OnlineCourse.Dtos;

public class LoginResponseDto
{
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required string RefreshToken { get; set; }
}
