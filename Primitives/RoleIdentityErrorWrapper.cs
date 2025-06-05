using Microsoft.AspNetCore.Identity;

namespace OnlineCourse.Primitives;

public class RoleIdentityErrorWrapper : Error
{
    public IEnumerable<IdentityError> IdentityErrors { get; init; }

    public RoleIdentityErrorWrapper(IEnumerable<IdentityError> identityErrors) : base(
        statusCode: StatusCodes.Status400BadRequest,
        code: "ROLE_ASSIGNMENT_FAILED",
        title: "User Role Assignment Failure",
        detail: "The designated user role could not be assigned. " +
        "This may be because the role does not exist in the system or the user already possesses this role.")
    {
        IdentityErrors = identityErrors;
    }

    public RoleIdentityErrorWrapper(IEnumerable<IdentityError> identityErrors, string role) :base(
        statusCode: StatusCodes.Status400BadRequest,
        code: "ROLE_ASSIGNMENT_FAILED",
        title: "User Role Assignment Failure",
        detail: $"The role '{role}' could not be assigned to the user. " +
        $"This may be because the role does not exist in the system or the user already possesses this role.")
    {
        IdentityErrors = identityErrors;
    }
}