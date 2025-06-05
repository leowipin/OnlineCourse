namespace OnlineCourse.Primitives;

public class InvalidRefreshTokenError() : Error(
        statusCode: StatusCodes.Status401Unauthorized,
        code: "AUTH_INVALID_REFRESH_TOKEN",
        title: "Invalid or expired refresh token",
        detail: "The provided refresh token is invalid, expired, or has been revoked.")
{ }
