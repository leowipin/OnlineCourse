namespace OnlineCourse.Primitives;

public abstract class Error(string code, string title, string detail)
{
    public string Code { get; } = code;
    public string Title { get; } = title;
    public string Detail { get; } = detail;
}