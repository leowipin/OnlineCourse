namespace OnlineCourse.Data.Constants;

public static class AppRoles
{
    public const string Instructor = "Instructor";
    public const string Student = "Student";
    public const string Admin = "Admin";

    public static readonly Guid InstructorRoleId = Guid.Parse("2b04f320-d015-458d-aea4-36fc2e21a3f3");
    public static readonly Guid StudentRoleId = Guid.Parse("0ed6fc08-9ae5-4302-bf34-b4e40b832f71");
    public static readonly Guid AdminRoleId = Guid.Parse("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e");
}