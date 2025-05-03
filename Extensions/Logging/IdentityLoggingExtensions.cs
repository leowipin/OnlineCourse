using OnlineCourse.Exceptions;
using OnlineCourse.Primitives;

namespace OnlineCourse.Extensions.Logging;

public static class IndentityLoggingExtensions
{
    public static void LogUserCreationIssue(
        this ILogger logger,
        UserIdentityErrorWrapper userError,
        string? endpointInfo = null)
    {
        const string logTemplate = "Problema en creación de usuario [{Endpoint}]: {Title} - {Detail}. " +
            "IdentityErrors: {@IdentityErrors}";

        logger.LogInformation(
            message: logTemplate,
            endpointInfo ?? "N/A",
            userError.Title,
            userError.Detail,
            userError.IdentityErrors);
    }
    public static void LogAssignRoleIssue(
        this ILogger logger,
        RoleIdentityErrorWrapper roleError,
        string? endpointInfo = null)
    {
        const string logTemplate = "Problema asignando rol [{Endpoint}]: {Title} - {Detail}. " +
            "IdentityErrors: {@IdentityErrors}";

        logger.LogInformation(
            message: logTemplate,
            endpointInfo ?? "N/A",
            roleError.Title,
            roleError.Detail,
            roleError.IdentityErrors);
    }
} 