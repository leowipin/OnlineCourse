using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Extensions.Logging;
using OnlineCourse.Primitives;

namespace OnlineCourse.Web;

public static class ControllerErrorHandlingExtensions
{
    public static ActionResult HandleServiceError(
        this ControllerBase controller,
        Error error,
        ILogger logger,
        string? endpointInfo = null)
    {
        string? instace = controller.HttpContext.Request.Path; 
        switch (error)
        {
            // ---- Identity errors ----
            case UserIdentityErrorWrapper userWrapper:
                logger.LogUserCreationIssue(userWrapper, endpointInfo);
                var userProblemDetails = CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    userWrapper.Title,
                    userWrapper.Detail,
                    instace
                );
                userProblemDetails.Extensions.Add("identityErrors", userWrapper.IdentityErrors);
                return controller.BadRequest(userProblemDetails);

            case RoleIdentityErrorWrapper roleWrapper:
                logger.LogAssignRoleIssue(roleWrapper, endpointInfo);
                var roleProblemDetails = CreateProblemDetails(
                   StatusCodes.Status400BadRequest,
                   roleWrapper.Title,
                   roleWrapper.Detail,
                   instace
               );
                roleProblemDetails.Extensions.Add("identityErrors", roleWrapper.IdentityErrors);
                return controller.BadRequest(roleProblemDetails);
            // -----------------------
            case InvalidCredentialsError invalidCredentialsError:
                logger.LogGenericServiceError(invalidCredentialsError, endpointInfo);
                var invalidCredProblemDetails = CreateProblemDetails(
                    StatusCodes.Status401Unauthorized, // Or 400 BadRequest depending on desired behavior
                    invalidCredentialsError.Title,
                    invalidCredentialsError.Detail,
                    instace
                );
                return controller.Unauthorized(invalidCredProblemDetails);

            case AccountLockedOutError accountLockedOutError:
                logger.LogGenericServiceError(accountLockedOutError, endpointInfo);
                var lockedOutProblemDetails = CreateProblemDetails(
                    StatusCodes.Status400BadRequest, // Or 403 Forbidden if more appropriate
                    accountLockedOutError.Title,
                    accountLockedOutError.Detail,
                    instace
                );
                return controller.BadRequest(lockedOutProblemDetails);

            case EmailNotConfirmedError emailNotConfirmedError:
                logger.LogGenericServiceError(emailNotConfirmedError, endpointInfo);
                var emailNotConfirmedProblemDetails = CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    emailNotConfirmedError.Title,
                    emailNotConfirmedError.Detail,
                    instace
                );
                return controller.BadRequest(emailNotConfirmedProblemDetails);

            case NotFoundError notFoundError:
                logger.LogGenericServiceError(notFoundError, endpointInfo);
                var notFoundProblemDetails = CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    notFoundError.Title,
                    notFoundError.Detail,
                    instace
                );
                return controller.NotFound(notFoundProblemDetails);
            default:
                logger.LogGenericServiceError(error, endpointInfo);
                var problemDetails = CreateProblemDetails(
                    StatusCodes.Status500InternalServerError,
                    error.Title,
                    error.Detail,
                    instace
                );
                return controller.StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
        }
    }
    private static ProblemDetails CreateProblemDetails(int status, string title, string detail, string? instance)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = instance,
        };
    }
}