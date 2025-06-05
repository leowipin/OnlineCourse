namespace OnlineCourse.Primitives;

public class GenericError(int statusCode, string code, string title, string detail) : 
    Error(statusCode, code, title, detail)
{}