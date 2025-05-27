namespace OnlineCourse.Primitives;

public class GenericError : Error
{
    public GenericError(string code, string title, string detail) : base(code, title, detail) { }
}