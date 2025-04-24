namespace OnlineCourse.Data.Constants;

public static class AppRoles
{
    // Name
    public const string Instructor = "Instructor";
    public const string Student = "Student";
    public const string Admin = "Admin";
    // Id
    public static readonly Guid InstructorRoleId = Guid.Parse("2b04f320-d015-458d-aea4-36fc2e21a3f3");
    public static readonly Guid StudentRoleId = Guid.Parse("0ed6fc08-9ae5-4302-bf34-b4e40b832f71");
    public static readonly Guid AdminRoleId = Guid.Parse("35d30a2a-d3b7-40d4-9c5e-cd2d4eda950e");
    // ConcurrencyStamp
    public const string ConcurrencyInstructor = "614676e4-09b4-4572-a391-c0d3739efa1d";
    public const string ConcurrencyStudent = "7be6161d-be1e-4306-a3dc-3fb038aa8c77";
    public const string ConcurrencyAdmin = "5ccfee95-5962-408a-96e3-35b5679d096c";
}