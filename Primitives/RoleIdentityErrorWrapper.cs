using Microsoft.AspNetCore.Identity;

namespace OnlineCourse.Primitives;

public class RoleIdentityErrorWrapper(IEnumerable<IdentityError> identityErrors) : 
    Error(title: "Error de configuración de usuario.",
          detail: "No se pudo asignar el rol requerido al usuario.")
{
    public IEnumerable<IdentityError> IdentityErrors { get; } = identityErrors;
}