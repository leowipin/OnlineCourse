using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Dtos
{
    public class InstructorCreationDto
    {
        [EmailAddress]
        public required string Email { get; init; }

        [StringLength(100,MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$")]
        public required string Password { get; init; }

        [MaxLength(2000)]
        public string? Biography { get; init; }

        [MaxLength(255)]
        public string? WebSiteUrl { get; init; }
    }
}