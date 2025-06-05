namespace OnlineCourse.Primitives;

public class ReusedRefreshTokenError() : Error(
    statusCode: StatusCodes.Status401Unauthorized,
    code: "AUTH_REUSED_REFRESH_TOKEN",
    title: "Reused refresh token detected",
    detail: "A refresh token that was already used has been presented."
    )
{}