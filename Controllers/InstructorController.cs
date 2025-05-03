using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Dtos;
using OnlineCourse.Extensions.Logging;
using OnlineCourse.Primitives;
using OnlineCourse.Services.IServices;
using OnlineCourse.Web;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController(IInstructorService instructorService, ILogger<InstructorController> logger) : ControllerBase
    {
        private readonly IInstructorService _instructorService = instructorService;
        private readonly ILogger<InstructorController> _logger = logger;

        /// <summary>
        /// Crea un nuevo instructor.
        /// </summary>
        /// <param name="instructorCreation">Datos necesarios para la creación del instructor.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>El instructor creado.</returns>
        /// <response code="201">Instructor creado exitosamente.</response>
        /// <response code="400">Error en los datos proporcionados para la creación del instructor.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [ProducesResponseType<InstructorDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorDto>> InstructorCreate(
            [FromBody] InstructorCreationDto instructorCreation,
            CancellationToken ct)
        {
            Result<InstructorDto> instructorDto = await _instructorService
                .CreateInstructorAsync(instructorCreation, ct);

            if (!instructorDto.IsSucces)
            {
                return this.HandleServiceError(instructorDto.Error!, _logger, nameof(InstructorCreate));
            }
            return CreatedAtAction(nameof(InstructorGet), new { id = instructorDto.Data?.Id }, instructorDto.Data);
        }

        /// <summary>
        /// Obtiene un instructor por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del instructor.</param>
        /// <param name="ct">Token de cancelación.</param>
        /// <returns>El instructor solicitado.</returns>
        /// <response code="200">Instructor encontrado exitosamente.</response>
        /// <response code="404">No se encontró un instructor con el identificador proporcionado.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType<InstructorDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorDto>> InstructorGet(Guid id, CancellationToken ct)
        {
            var instructorDto = await _instructorService.GetInstructorByIdAsync(id, ct);
            if (instructorDto == null)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Usuario no encontrado.",
                    Detail = "No se encontró un instructor con el identificador proporcionado."
                };
                return NotFound(problemDetails);
            }
            return Ok(instructorDto);
        }
    }
}