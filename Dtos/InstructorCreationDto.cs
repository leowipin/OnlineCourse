using System.ComponentModel.DataAnnotations;

namespace OnlineCourse.Dtos
{
    public class InstructorCreationDto
    {
        [Required(ErrorMessage = "El correo electr�nico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electr�nico no es v�lido.")]
        public required string Email { get; init; }

        [Required(ErrorMessage = "La contrase�a es obligatoria.")]
        [StringLength(100, ErrorMessage = "La contrase�a debe tener al menos {2} caracteres", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$",
        ErrorMessage = "La contrase�a debe contener al menos una letra min�scula, una letra may�scula, un n�mero y un car�cter especial")]
        public required string Password { get; init; }

        [MaxLength(2000, ErrorMessage = "La biograf�a no puede exceder los 2000 caracteres.")]
        public string? Biography { get; init; }

        [MaxLength(255, ErrorMessage = "La URL del sitio web no puede exceder los 255 caracteres.")]
        public string? WebSiteUrl { get; init; }
    }
}