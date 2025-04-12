using OnlineCourse.Entities.Base;

namespace OnlineCourse.Entities
{
    public class CourseTag : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        // nav property
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}