using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Data.Constants;

namespace OnlineCourse.Configurations;

public class RoleConfig : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        // Seeding
        builder.HasData(

            new IdentityRole<Guid>
            {
                Id = AppRoles.InstructorRoleId,
                Name = AppRoles.Instructor,
                NormalizedName = AppRoles.Instructor.ToUpperInvariant(),
                ConcurrencyStamp = AppRoles.ConcurrencyInstructor,
            },

            new IdentityRole<Guid>
            {
                Id = AppRoles.StudentRoleId,
                Name = AppRoles.Student,
                NormalizedName = AppRoles.Student.ToUpperInvariant(),
                ConcurrencyStamp = AppRoles.ConcurrencyStudent,
            },

            new IdentityRole<Guid>
            {
                Id = AppRoles.AdminRoleId,
                Name = AppRoles.Admin,
                NormalizedName = AppRoles.Admin.ToUpperInvariant(),
                ConcurrencyStamp = AppRoles.ConcurrencyAdmin,
            }

            );
    }
}