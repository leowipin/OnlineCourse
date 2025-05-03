using Microsoft.AspNetCore.Identity;

namespace OnlineCourse.Primitives;

public class UserIdentityErrorWrapper(IEnumerable<IdentityError> identityErrors) : 
    Error(title: "Error al crear el usuario.",
          detail: "Fall� la validaci�n de los datos del usuario.")
{
    public IEnumerable<IdentityError> IdentityErrors { get; } = identityErrors;
}