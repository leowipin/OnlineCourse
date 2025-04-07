using OnlineCourse.Interfaces;

namespace OnlineCourse.Entities.Base
{
    public abstract class AuditableEntity : IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}