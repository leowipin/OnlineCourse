namespace OnlineCourse.Primitives;

public class Result<T>
{
    public bool IsSucces { get; }
    public T? Data { get; }
    public Error? Error { get; }
    private Result(bool isSucces, T? data, Error? error) 
    { 
        IsSucces = isSucces;
        Data = data;
        Error = error;
    }
    // Factory
    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(Error error) => new(false, default, error);
}