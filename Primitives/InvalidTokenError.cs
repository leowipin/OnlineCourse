namespace OnlineCourse.Primitives;

public record InvalidTokenError(string Message = "The provided token is invalid, expired, or has been revoked.") 
    : Error("Authentication.InvalidToken", Message);
