namespace OnlineCourse.Primitives;

public class InvalidTokenError() : Error(
        code: "AUTH_INVALID_TOKEN",
        title: "Invalid or Expired Token",
        detail: "The provided authentication token is invalid, expired, or has been revoked.")
{ }
