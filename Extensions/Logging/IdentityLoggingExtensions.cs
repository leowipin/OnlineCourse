using OnlineCourse.Exceptions;

namespace OnlineCourse.Extensions.Logging;

public static class IndentityLoggingExtensions
{
    public static void LogUserCreationWarning(
        this ILogger logger,
        UserCreationException uce,
        string? endpointInfo = null)
    {
        const string logTemplate = "Advertencia de creación de usuario fallida en {EndpointInfo}: " +
                               "{ErrorTitle}. Detalles: {ErrorMessage}. IdentityErrors: {@IdentityErrors}";

        logger.LogWarning(exception: uce,
                     message: logTemplate,
                     endpointInfo ?? "N/A",
                     uce.Title,
                     uce.Message,
                     uce.IdentityErrors);
    }
} 