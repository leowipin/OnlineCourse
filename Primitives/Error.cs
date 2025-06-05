namespace OnlineCourse.Primitives;

public abstract class Error(int statusCode, string code, string title, string detail)
{
    public int StatusCode { get; } = statusCode;
    public string Code { get; } = code;
    public string Title { get; } = title;
    public string Detail { get; } = detail;
}