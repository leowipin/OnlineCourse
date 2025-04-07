using OnlineCourse.Entities.Base;
using OnlineCourse.Enums;

namespace OnlineCourse.Entities
{
    public class Student : AuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public StudentTypes StudentType { get; set; }
        // nav properties
        public User User { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}