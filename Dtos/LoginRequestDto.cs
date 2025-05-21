using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Dtos;

public class LoginRequestDto
{
    [EmailAddress]
    public required string Email { get; set; }

    public required string Password { get; set; }
}
