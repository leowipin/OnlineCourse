using OnlineCourse.Entities.Base;

namespace OnlineCourse.Entities
{
    public class Instructor : AuditableEntity
    {
        public Guid Id { get; set; }
        public string? Biography { get; set; }
        public string? WebSiteUrl { get; set; }
        // nav property
        public User User { get; set; } = null!;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}