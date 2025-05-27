using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Entities;

namespace OnlineCourse.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Properties
            builder.Property(x => x.Email).IsRequired();
            // Relations
            builder.HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(u => u.Instructor)
                .WithOne(i => i.User)
                .HasForeignKey<Instructor>(i=>i.Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}