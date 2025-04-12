using OnlineCourse.Entities.Base;

namespace OnlineCourse.Entities
{
    public class Lesson : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? VideorUrl { get; set; }
        public string Content { get; set; } = null!;
        public int DurationSeconds { get; set; }
        public byte Order { get; set; }
        // foreign key
        public Guid ModuleId { get; set; }
        // nav properties
        public Module Module { get; set; } = null!;
    }
}