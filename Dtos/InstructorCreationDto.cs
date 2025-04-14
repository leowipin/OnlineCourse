using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Dtos
{
    public class InstructorCreationDto
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public required string Email { get; init; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$",
        ErrorMessage = "La contraseña debe contener al menos una letra minúscula, una letra mayúscula, un número y un carácter especial")]
        public required string Password { get; init; }

        [MaxLength(2000, ErrorMessage = "La biografía no puede exceder los 2000 caracteres.")]
        public string? Biography { get; init; }

        [MaxLength(255, ErrorMessage = "La URL del sitio web no puede exceder los 255 caracteres.")]
        public string? WebSiteUrl { get; init; }
    }
}