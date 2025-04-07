using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;

namespace OnlineCourse.Configurations
{
    public class CourseTagConfig : IEntityTypeConfiguration<CourseTag>
    {
        public void Configure(EntityTypeBuilder<CourseTag> builder)
        {
            // Table name
            builder.ToTable("CourseTags");
            // Indexes
            builder.HasIndex(x => x.Name).IsUnique();
            // Properties
            builder.Property(x => x.Name).HasMaxLength(50);
        }
    }
}