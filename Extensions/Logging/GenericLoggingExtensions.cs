using OnlineCourse.Primitives;

namespace OnlineCourse.Extensions.Logging;

public static class GenericLoggingExtensions
{
    public static void LogServiceEvent(
        this ILogger logger,
        Error error,
        LogLevel level,
        string? endpointInfo = null
        )
    {
        const string template = "[{Endpoint}]: {Title} - {Detail} - {Code}.";
        logger.Log(
            logLevel: level,
            message: template,
            endpointInfo ?? "N/A",
            error.Code,
            error.Title,
            error.Detail);
    }
}