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
        switch (error)
        {
            // Identity errors
            case UserIdentityErrorWrapper userWrapper:
                logger.LogUserCreationIssue(userWrapper, endpointInfo);
                var userProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = userWrapper.Title,
                    Detail = userWrapper.Detail,
                };
                userProblemDetails.Extensions.Add("identityErrors", userWrapper.IdentityErrors);
                return controller.BadRequest(userProblemDetails);

            case RoleIdentityErrorWrapper roleWrapper:
                logger.LogAssignRoleIssue(roleWrapper, endpointInfo);
                var roleProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = roleWrapper.Title,
                    Detail = roleWrapper.Detail,
                };
                roleProblemDetails.Extensions.Add("identityErrors", roleWrapper.IdentityErrors);
                return controller.BadRequest(roleProblemDetails);

            default:
                logger.LogGenericServiceError(error, endpointInfo);
                var genericProblemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = error.Title,
                    Detail = error.Detail,
                };
                return controller.StatusCode(StatusCodes.Status500InternalServerError, genericProblemDetails);
        }
    } 
}