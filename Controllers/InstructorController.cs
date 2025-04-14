using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Dtos;
using OnlineCourse.Exceptions;
using OnlineCourse.Services.IServices;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController(IInstructorService instructorService) : ControllerBase
    {
        private readonly IInstructorService _instructorService = instructorService;

        [HttpPost]
        [ProducesResponseType<InstructorDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorDto>> InstructorCreate(
            [FromBody] InstructorCreationDto instructorCreation,
            CancellationToken ct)
        {
            try
            {
                InstructorDto instructorDto = await _instructorService
                    .CreateInstructorAsync(instructorCreation, ct);
                return CreatedAtAction(nameof(InstructorGet), new { id = instructorDto.Id }, instructorDto);
            }
            catch (UserCreationException uce)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = uce.Title,
                    Detail = uce.Message
                };
                problemDetails.Extensions.Add("identityErrors", uce.IdentityErrors);
                return BadRequest(problemDetails);
            }
            catch (Exception)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error interno del servidor.",
                    Detail = "Ocurrió un error inesperado. Por favor, intenta nuevamente más tarde."
                };
                return StatusCode(StatusCodes.Status500InternalServerError, problemDetails);
            }
        }
        [HttpGet("{id}")]
        [ProducesResponseType<InstructorDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorDto>> InstructorGet(Guid id, CancellationToken ct)
        {
            var instructorDto = await _instructorService.GetInstructorByIdAsync(id, ct);
            if(instructorDto == null)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Usuario no encontrado.",
                    Detail = "No se encontró un instructor con el identificador proporcionado."
                };
                return NotFound(problemDetails);
            }
            ;
            return Ok(instructorDto);
        }
    }
}