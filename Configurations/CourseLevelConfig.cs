using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;

namespace OnlineCourse.Configurations
{
    public class CourseLevelConfig : IEntityTypeConfiguration<CourseLevel>
    {
        public void Configure(EntityTypeBuilder<CourseLevel> builder)
        {
            // Table name
            builder.ToTable("CourseLevels");
            // Indexes
            builder.HasIndex(cl => cl.Name).IsUnique();
            // Properties
            builder.Property(cl => cl.Name).HasMaxLength(50);
            builder.Property(cl => cl.Description).HasMaxLength(200);
        }
    }
}
