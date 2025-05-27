namespace OnlineCourse.Primitives;

public class InvalidCredentialsError(string email) : Error(
    code: "AUTH_INVALID_CREDENTIALS",
    title: "Invalid Email or Password Provided",
    detail: $"The credentials provided for email '{email}' are incorrect.")
{ }
