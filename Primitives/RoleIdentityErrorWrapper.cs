using Microsoft.AspNetCore.Identity;

namespace OnlineCourse.Primitives;

public class RoleIdentityErrorWrapper(IEnumerable<IdentityError> identityErrors, string role) :
    Error(
        code: "ROLE_ASSIGNMENT_FAILED",
        title: "User Role Assignment Failure",
        detail: $"The role '{role}' could not be assigned to the user. " +
        $"This may be because the role does not exist in the system or the user already possesses this role.")
{
    public IEnumerable<IdentityError> IdentityErrors { get; } = identityErrors;
}