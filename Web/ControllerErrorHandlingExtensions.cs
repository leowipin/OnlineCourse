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
                var userProblemDetails = CreateProblemDetails(userWrapper,instace);
                userProblemDetails.Extensions.Add("identityErrors", userWrapper.IdentityErrors);
                return controller.StatusCode(userWrapper.StatusCode, userProblemDetails);

            case RoleIdentityErrorWrapper roleWrapper:
                var roleProblemDetails = CreateProblemDetails(roleWrapper,instace);
                roleProblemDetails.Extensions.Add("identityErrors", roleWrapper.IdentityErrors);
                return controller.StatusCode(roleWrapper.StatusCode, roleProblemDetails);
            // -----------------------
            default:
                var problemDetails = CreateProblemDetails(error,instace);
                return controller.StatusCode(error.StatusCode, problemDetails);
        }
         
    }
    private static ProblemDetails CreateProblemDetails(Error error, string? instance)
    {
        var problemDetails = new ProblemDetails
        {
            Status = error.StatusCode,
            Title = error.Title,
            Detail = error.Detail,
            Instance = instance,
        };
        problemDetails.Extensions["code"] = error.Code;
        return problemDetails;

    }
}