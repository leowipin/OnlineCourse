using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Dtos
{
    public class InstructorCreationDto
    {
        [Required]
        public required string Email { get; init; }
        [Required]
        public required string Password { get; init; }
        public string? Biography { get; init; }
        public string? WebSiteUrl { get; init; }
    }
}