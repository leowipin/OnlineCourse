using OnlineCourse.Entities.Base;

namespace OnlineCourse.Entities
{
    public class CourseLevel : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        // Nav Property
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
