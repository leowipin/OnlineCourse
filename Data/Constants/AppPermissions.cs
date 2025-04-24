namespace OnlineCourse.Data.Constants;

public static class AppPermissions
{
    public static class Courses
    {
        public const string Create = "Permissions.Courses.Create";
        public const string Read = "Permissions.Courses.Read";
        public const string Update = "Permissions.Courses.Update";
        public const string Delete = "Permissions.Courses.Delete";

        public const string ReadOwn = "Permissions.Courses.ReadOwn";
        public const string UpdateOwn = "Permissions.Courses.UpdateOwn";
        public const string DeleteOwn = "Permissions.Courses.DeleteOwn";

        public const string ManageAll = "Permissions.Courses.ManageAll";
    }
    public static class Enrollments
    {
        public const string Create = "Permissions.Enrollments.Create";
        public const string Read = "Permissions.Enrollments.Read";
        public const string Update = "Permissions.Enrollments.Update";
        public const string Delete = "Permissions.Enrollments.Delete";

        public const string ReadOwn = "Permissions.Enrollments.ReadOwn";
        public const string UpdateOwn = "Permissions.Enrollments.UpdateOwn";
        public const string DeleteOwn = "Permissions.Enrollments.DeleteOwn";

        public const string ManageAll = "Permissions.Enrollments.ManageAll";
    }
    public static class Users
    {
        public const string Read = "Permissions.Users.Read";

        public const string Manage = "Permissions.Users.Manage";
    }
    public static class Roles
    {
        public const string Read = "Permissions.Roles.Read";
        public const string Manage = "Permissions.Roles.Manage";
    }
    public static class CourseContent
    {
        public const string ReadEnrolledCourse = "Permissions.CourseContent.ReadEnrolledCourse";
        public const string ManageOwnCourse = "Permissions.CourseContent.ManageOwnCourse";
        public const string ManageAll = "Permissions.CourseContent.ManageAll";
    }
}