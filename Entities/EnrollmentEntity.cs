using OnlineCourse.Entities.Base;

namespace OnlineCourse.Entities
{
    public class Enrollment : AuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal ProgressPercentage { get; set; }
        // foreign keys
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        // nav properties
        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}