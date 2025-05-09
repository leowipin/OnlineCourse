using OnlineCourse.Primitives;

namespace OnlineCourse.Extensions.Logging;

public static class GenericLoggingExtensions
{
    public static void LogGenericServiceError(
        this ILogger logger,
        Error error,
        string? endpointInfo = null
        )
    {
        const string template = "[{Endpoint}]: {Title} - {Detail}.";
        logger.LogWarning(
            message: template,
            endpointInfo ?? "N/A",
            error.Title,
            error.Detail);
    }
}