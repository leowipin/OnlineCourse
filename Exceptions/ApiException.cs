namespace OnlineCourse.Exceptions;

public class ApiException(
    string errorCode,
    string errorTitle,
    string message,
    int errorStatus = StatusCodes.Status500InternalServerError) : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
    public string ErrorTitle { get; } = errorTitle;
    public int ErrorStatus { get; } = errorStatus;
}