namespace OnlineCourse.Dtos;

public record InstructorDto
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string? Biography { get; init; }
    public required string? WebSiteUrl { get; init; }
}