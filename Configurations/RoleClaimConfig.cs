using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCourse.Data.Constants;

namespace OnlineCourse.Configurations;

public class RoleClaimConfig : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        int roleClaimCount = 1;

        // Seeding
        builder.HasData(
            // Admin Role Claims
            // Courses
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.AdminRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Courses.ManageAll
            },

            // Enrollments
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.AdminRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Enrollments.ManageAll
            },

            // Users
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.AdminRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Users.Read
            },
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.AdminRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Users.Manage
            },

            // Roles
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.AdminRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Roles.Read
            },
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.AdminRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Roles.Manage
            },

            // CourseContent
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.AdminRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.CourseContent.ManageAll
            },

            // Student Role Claims
            // Courses
            new IdentityRoleClaim<Guid> 
            { 
                Id = roleClaimCount++, 
                RoleId = AppRoles.StudentRoleId, 
                ClaimType = AppClaimTypes.Permission, 
                ClaimValue = AppPermissions.Courses.Read 
            },
            // Enrollments
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.StudentRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Enrollments.Create
            },
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.StudentRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Enrollments.ReadOwn
            },

            // CourseContent
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.StudentRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.CourseContent.ReadEnrolledCourse
            },

            // Instructor Role Claims
            // Courses
            new IdentityRoleClaim<Guid> 
            { 
                Id = roleClaimCount++, 
                RoleId = AppRoles.InstructorRoleId, 
                ClaimType = AppClaimTypes.Permission, 
                ClaimValue = AppPermissions.Courses.Create
            },
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.InstructorRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Courses.ReadOwn
            },
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.InstructorRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Courses.UpdateOwn
            },
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.InstructorRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Courses.DeleteOwn
            },
            new IdentityRoleClaim<Guid> 
            { 
                Id = roleClaimCount++,
                RoleId = AppRoles.InstructorRoleId, 
                ClaimType = AppClaimTypes.Permission, 
                ClaimValue = AppPermissions.Courses.Read 
            },
            // Enrollments
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.InstructorRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Enrollments.Read
            },
          
            // Users
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.InstructorRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.Users.Read
            },

            // CourseContent
            new IdentityRoleClaim<Guid>
            {
                Id = roleClaimCount++,
                RoleId = AppRoles.InstructorRoleId,
                ClaimType = AppClaimTypes.Permission,
                ClaimValue = AppPermissions.CourseContent.ManageOwnCourse
            }
        );
    }
}