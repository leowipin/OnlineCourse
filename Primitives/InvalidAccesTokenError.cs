namespace OnlineCourse.Primitives;

public class InvalidAccesTokenError() : Error(
        statusCode: StatusCodes.Status401Unauthorized,
        code: "AUTH_INVALID_ACCES_TOKEN",
        title: "Invalid or expired Access token.",
        detail: "The access token provided is either invalid, malformed, or has expired.")
{ }