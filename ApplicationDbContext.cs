using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Configurations;
using OnlineCourse.Entities;
using OnlineCourse.Entities.Base;

namespace OnlineCourse
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) { }
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<CourseTag> CourseTags { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Rename Identity tables
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            // Configuration files
            builder.ApplyConfiguration(new CourseConfig());
            builder.ApplyConfiguration(new CourseTagConfig());
            builder.ApplyConfiguration(new EnrollmentConfig());
            builder.ApplyConfiguration(new InstructorConfig());
            builder.ApplyConfiguration(new LessonConfig());
            builder.ApplyConfiguration(new ModuleConfig());
            builder.ApplyConfiguration(new StudentConfig());
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new CourseLevelConfig());

            // Global DeletedAt filter
            foreach(var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsAssignableFrom(typeof(IAuditableEntity))
                    && typeof(User) != entityType.ClrType
                    && typeof(Student) != entityType.ClrType
                    && typeof(Instructor) != entityType.ClrType) 
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, "DeletedAt");
                    var nullConstant = Expression.Constant(null, property.Type);
                    var equality = Expression.Equal(property, nullConstant);
                    var lambda = Expression.Lambda(equality, parameter);
                    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
                if (entityType.FindProperty("CreatedAt") != null)
                {
                    // Default value of CreatedAt
                    builder.Entity(entityType.ClrType)
                        .Property("CreatedAt")
                        .HasDefaultValueSql("GETUTCDATE()");
                    
                }
                if(entityType.FindProperty("DeletedAt") != null)
                {
                    // Create index on DeletedAt
                    builder.Entity(entityType.ClrType).HasIndex("DeletedAt");
                }
            }
        }
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(ct);
        }
        public void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<IAuditableEntity>();

            foreach(var entry in entries)
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                if(entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                // Soft delete
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                }
            }
        }
    }
}