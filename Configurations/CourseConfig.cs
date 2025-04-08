using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;

namespace OnlineCourse.Configurations
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // Constraints values
            var priceColumnName = "Price";
            // Table name & constraints
            builder.ToTable("Courses", b => b.HasCheckConstraint(
                    name: "CK_Courses_Price",
                    sql: $"[{priceColumnName}] > 0"
                ));
            // Properties
            builder.Property(x => x.Title).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            // Relations
            builder.HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorProfileId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.CourseLevel)
                .WithMany(cl => cl.Courses)
                .HasForeignKey(c => c.CourseLevelId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.CourseTags)
                .WithMany(ct => ct.Courses)
                .UsingEntity(j => j.ToTable("Course_CourseTag"));
        }
    }
}