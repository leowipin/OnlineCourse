using Microsoft.AspNetCore.Identity;

namespace OnlineCourse.Exceptions
{
    public class UserCreationException : Exception
    {
        public IEnumerable<IdentityError> IdentityErrors { get; }
    
        public UserCreationException(IEnumerable<IdentityError> errors)
            : base("Error al crear el usuario")
        {
            IdentityErrors = errors;
        }
    }
}