using OnlineCourse.Entities.Base;
using OnlineCourse.Enums;

namespace OnlineCourse.Entities
{
    public class Course : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime? PublishedDate { get; set; }
        public CourseLevels Level { get; set; }
        // foreign keys
        public Guid InstructorProfileId { get; set; }
        // nav property
        public InstructorProfile Instructor { get; set; } = null!;
        public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}