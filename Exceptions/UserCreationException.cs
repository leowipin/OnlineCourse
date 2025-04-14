using Microsoft.AspNetCore.Identity;

namespace OnlineCourse.Exceptions
{
    public class UserCreationException : Exception
    {
        public IEnumerable<IdentityError> IdentityErrors { get; }
        public string Title { get; }
    
        public UserCreationException(IEnumerable<IdentityError> errors, string title )
            : base("Falló la validación de los datos del usuario.")
        {
            IdentityErrors = errors;
            Title = title;
        }
    }
}