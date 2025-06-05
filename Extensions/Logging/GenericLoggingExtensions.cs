using OnlineCourse.Primitives;

namespace OnlineCourse.Extensions.Logging;

public static class GenericLoggingExtensions
{
    public static void LogServiceEvent(
        this ILogger logger,
        Error error,
        LogLevel level,
        string? endpointInfo = null,
        Exception? ex = null
        )
    {
        const string template = "[{Endpoint}]: {Title} - {Detail} - {Code}.";
        logger.Log(
            logLevel: level,
            exception: ex,
            message: template,
            endpointInfo ?? "N/A",
            error.Code,
            error.Title,
            error.Detail);
    }
}