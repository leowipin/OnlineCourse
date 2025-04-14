using OnlineCourse.Entities.Base;

namespace OnlineCourse.Entities
{
    public class Course : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime? PublishedDate { get; set; }
        // foreign keys
        public Guid InstructorProfileId { get; set; }
        public Guid CourseLevelId { get; set; }
        // nav property
        public Instructor Instructor { get; set; } = null!;
        public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public CourseLevel CourseLevel { get; set; } = null!;

    }
}