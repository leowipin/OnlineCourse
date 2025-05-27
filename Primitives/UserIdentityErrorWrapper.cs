using Microsoft.AspNetCore.Identity;

namespace OnlineCourse.Primitives;

public class UserIdentityErrorWrapper(IEnumerable<IdentityError> identityErrors) :
    Error(
        code: "USER_CREATION_FAILED",
        title: "User Creation Validation Failure",
        detail: "User creation failed due to validation errors in the provided user data.")
{
    public IEnumerable<IdentityError> IdentityErrors { get; } = identityErrors;
}