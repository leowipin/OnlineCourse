using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;

namespace OnlineCourse.Configurations
{
    public class EnrollmentConfig : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            // Constraint values
            var progressColumnName = "ProgressPercentage";
            // Table name & constraints
            builder.ToTable("Enrollments", b => b.HasCheckConstraint(
                    name: "CK_Enrollments_Progress",
                    sql: $"[{progressColumnName}] BETWEEN 0 AND 100"
                ));
            // Indexes
            // el estudiante no puede estar enrolado en el mismo curso dos veces
            builder.HasIndex(x=> new { x.StudentId, x.CourseId }).IsUnique();
            // Properties
            builder.Property(x => x.EnrollmentDate).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(x => x.ProgressPercentage)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0.00m);
            // Relations
            builder.HasOne(e=>e.Student)
                .WithMany(s=>s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Course)
                .WithMany(c=>c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}