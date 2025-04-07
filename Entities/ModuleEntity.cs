using OnlineCourse.Entities.Base;

namespace OnlineCourse.Entities
{
    public class Module : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public byte Order { get; set; }
        // foreing Keys
        public Guid CourseId { get; set; }
        // nav properties
        public Course Course { get; set; } = null!;
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}