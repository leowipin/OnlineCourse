using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Dtos;

public record LoginRequestDto
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string Password { get; init; }
}