using Microsoft.AspNetCore.Identity;
using OnlineCourse.Entities.Base;

namespace OnlineCourse.Entities
{
    public class User : IdentityUser<Guid>, IAuditableEntity
    {
        // auditable fields
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        // nav properties
        public Student? Student { get; set; }
        public Instructor? Instructor { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}