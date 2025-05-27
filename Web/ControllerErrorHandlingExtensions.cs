using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Extensions.Logging;
using OnlineCourse.Primitives;

namespace OnlineCourse.Web;

public static class ControllerErrorHandlingExtensions
{
    public static ActionResult HandleServiceError(
        this ControllerBase controller,
        Error error)
    {
        string? instace = controller.HttpContext.Request.Path; 
        switch (error)
        {
            // ---- Identity errors ----
            case UserIdentityErrorWrapper userWrapper:
                var userProblemDetails = CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    userWrapper,
                    instace
                );
                userProblemDetails.Extensions.Add("identityErrors", userWrapper.IdentityErrors);
                return controller.BadRequest(userProblemDetails);

            case RoleIdentityErrorWrapper roleWrapper:
                var roleProblemDetails = CreateProblemDetails(
                   StatusCodes.Status400BadRequest,
                   roleWrapper,
                   instace
               );
                roleProblemDetails.Extensions.Add("identityErrors", roleWrapper.IdentityErrors);
                return controller.BadRequest(roleProblemDetails);
            // -----------------------
            case InvalidCredentialsError invalidCredentialsError:
                var invalidCredProblemDetails = CreateProblemDetails(
                    StatusCodes.Status401Unauthorized,
                    invalidCredentialsError,
                    instace
                );
                return controller.Unauthorized(invalidCredProblemDetails);

            case AccountLockedOutError accountLockedOutError:
                var lockedOutProblemDetails = CreateProblemDetails(
                    StatusCodes.Status403Forbidden,
                    accountLockedOutError,
                    instace
                );
                return controller.BadRequest(lockedOutProblemDetails);

            case EmailNotConfirmedError emailNotConfirmedError:
                var emailNotConfirmedProblemDetails = CreateProblemDetails(
                    StatusCodes.Status400BadRequest,
                    emailNotConfirmedError,
                    instace
                );
                return controller.BadRequest(emailNotConfirmedProblemDetails);

            case NotFoundError notFoundError:
                var notFoundProblemDetails = CreateProblemDetails(
                    StatusCodes.Status404NotFound,
                    notFoundError,
                    instace
                );
                return controller.NotFound(notFoundProblemDetails);
            default:
                var problemDetails = CreateProblemDetails(
                    StatusCodes.Status500InternalServerError,
                    error,
                    instace
                );
                return controller.StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
        }
    }
    private static ProblemDetails CreateProblemDetails(int status, Error error, string? instance)
    {
        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = error.Title,
            Detail = error.Detail,
            Instance = instance,
        };
        problemDetails.Extensions["code"] = error.Code;
        return problemDetails;

    }
}