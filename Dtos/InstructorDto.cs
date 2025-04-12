namespace OnlineCourse.Dtos
{
    public class InstructorDto
    {
        public Guid Id { get; init; }
        public required string Email { get; init; }
        public required string Biography { get; init; }
        public required string WebSiteUrl { get; init; }
    }
}