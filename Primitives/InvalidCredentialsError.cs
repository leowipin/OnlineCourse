namespace OnlineCourse.Primitives;

public class InvalidCredentialsError() : Error(
    title: "Authentication.InvalidCredentials",
    detail: "The email or password provided is incorrect.");
