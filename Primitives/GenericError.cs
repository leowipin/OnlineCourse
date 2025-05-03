namespace OnlineCourse.Primitives;

public class GenericError : Error
{
    public GenericError(string title, string detail) : base(title, detail) { }
}