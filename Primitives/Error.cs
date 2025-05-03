namespace OnlineCourse.Primitives;

public abstract class Error(string title, string detail)
{
    public string Title { get; } = title;
    public string Detail { get; } = detail;
}