namespace OnlineCourse.Primitives;

public class EmailNotConfirmedError() : Error(
    title: "Authentication.EmailNotConfirmed",
    detail: "The email for this account has not been confirmed. Please check your inbox.");
