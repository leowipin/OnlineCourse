using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Dtos;
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
            return CreatedAtAction(nameof(InstructorGet), new { id = instructorDto.Data!.Id }, instructorDto.Data);
        }

        /// <summary>
        /// Obtiene un instructor por su identificador.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType<InstructorDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorDto>> InstructorGet(Guid id, CancellationToken ct)
        {
            var instructorDto = await _instructorService.GetInstructorByIdAsync(id, ct);
            if (!instructorDto.IsSucces)
            {
                return this.HandleServiceError(instructorDto.Error!, _logger, nameof(InstructorGet));
            }
            return Ok(instructorDto.Data);
        }
    }
}