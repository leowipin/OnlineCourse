namespace OnlineCourse.Primitives;

public class NotFoundError(string resourceName, Guid id) : Error(
    statusCode: StatusCodes.Status404NotFound,
    code: "RESOURCE_NOT_FOUND",
    title: "Resource Not Found",
    detail: $"The resource '{resourceName}' with ID '{id}' could not be found.")
{}