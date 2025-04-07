using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;

namespace OnlineCourse.Configurations
{
    public class ModuleConfig : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            // To table & constraints
            builder.ToTable("Modules");
            // Indexes
            builder.HasIndex(x => new { x.CourseId, x.Order }).IsUnique();
            // Properties
            builder.Property(x => x.Title).HasMaxLength(150);
            // Relations
            builder.HasOne(m=>m.Course)
                .WithMany(c=>c.Modules)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}