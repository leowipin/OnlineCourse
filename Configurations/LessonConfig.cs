using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;

namespace OnlineCourse.Configurations
{
    public class LessonConfig : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            // Constraint values
            var durationColumnName = "DurationSeconds";
            // Table name & Constraint
            builder.ToTable("Lessons", b=>b.HasCheckConstraint(
                    name: "CK_Lessons_Duration",
                    sql: $"([{durationColumnName}] IS NULL)" + //can omit, int does not admit null
                    $"OR " +
                    $"([{durationColumnName}] > 0)"
                ));
            // Indexes
            builder.HasIndex(x => new { x.ModuleId, x.Order }).IsUnique();
            // Properties
            builder.Property(x => x.Title).HasMaxLength(200);
            builder.Property(x=>x.VideorUrl).HasMaxLength(500);
            builder.Property(x=>x.Content).HasMaxLength(4000);
            // Relations
            builder.HasOne(l => l.Module)
                .WithMany(m => m.Lessons)
                .HasForeignKey(l => l.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}