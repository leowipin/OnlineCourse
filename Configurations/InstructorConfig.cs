using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;

namespace OnlineCourse.Configurations
{
    public class InstructorConfig : IEntityTypeConfiguration<InstructorProfile>
    {
        public void Configure(EntityTypeBuilder<InstructorProfile> builder)
        {
            // Table name
            builder.ToTable("InstructorProfiles");
            // Properties
            builder.Property(x => x.Biography).HasMaxLength(2000);
            builder.Property(x => x.WebSiteUrl).HasMaxLength(255);
        }
    }
}