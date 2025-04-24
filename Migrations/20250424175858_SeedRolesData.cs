using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineCourse.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("0ed6fc08-9ae5-4302-bf34-b4e40b832f71"), "7be6161d-be1e-4306-a3dc-3fb038aa8c77", "Student", "STUDENT" },
                    { new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3"), "614676e4-09b4-4572-a391-c0d3739efa1d", "Instructor", "INSTRUCTOR" },
                    { new Guid("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e"), "5ccfee95-5962-408a-96e3-35b5679d096c", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permission", "Permissions.Courses.ManageAll", new Guid("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e") },
                    { 2, "Permission", "Permissions.Enrollments.ManageAll", new Guid("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e") },
                    { 3, "Permission", "Permissions.Users.Read", new Guid("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e") },
                    { 4, "Permission", "Permissions.Users.Manage", new Guid("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e") },
                    { 5, "Permission", "Permissions.Roles.Read", new Guid("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e") },
                    { 6, "Permission", "Permissions.Roles.Manage", new Guid("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e") },
                    { 7, "Permission", "Permissions.CourseContent.ManageAll", new Guid("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e") },
                    { 8, "Permission", "Permissions.Courses.Read", new Guid("0ed6fc08-9ae5-4302-bf34-b4e40b832f71") },
                    { 9, "Permission", "Permissions.Enrollments.Create", new Guid("0ed6fc08-9ae5-4302-bf34-b4e40b832f71") },
                    { 10, "Permission", "Permissions.Enrollments.ReadOwn", new Guid("0ed6fc08-9ae5-4302-bf34-b4e40b832f71") },
                    { 11, "Permission", "Permissions.CourseContent.ReadEnrolledCourse", new Guid("0ed6fc08-9ae5-4302-bf34-b4e40b832f71") },
                    { 12, "Permission", "Permissions.Courses.Create", new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3") },
                    { 13, "Permission", "Permissions.Courses.ReadOwn", new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3") },
                    { 14, "Permission", "Permissions.Courses.UpdateOwn", new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3") },
                    { 15, "Permission", "Permissions.Courses.DeleteOwn", new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3") },
                    { 16, "Permission", "Permissions.Courses.Read", new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3") },
                    { 17, "Permission", "Permissions.Enrollments.Read", new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3") },
                    { 18, "Permission", "Permissions.Users.Read", new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3") },
                    { 19, "Permission", "Permissions.CourseContent.ManageOwnCourse", new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0ed6fc08-9ae5-4302-bf34-b4e40b832f71"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2b04f320-d015-458d-aea4-36fc2e21a3f3"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e"));
        }
    }
}
