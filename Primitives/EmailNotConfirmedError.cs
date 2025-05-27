namespace OnlineCourse.Primitives;

public class EmailNotConfirmedError(string email) : Error(
    code: "AUTH_EMAIL_NOT_CONFIRMED",
    title: "Email Address Not Confirmed",
    detail: $"The email address '{email}' associated with this account has not been confirmed.")
{ }
