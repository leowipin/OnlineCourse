using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Dtos;
using OnlineCourse.Services.IServices;
using OnlineCourse.Web;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController(IInstructorService instructorService) : ControllerBase
    {
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
            var result = await instructorService.CreateInstructorAsync(
                instructorCreation,
                nameof(InstructorCreate),
                ct);
            return result.Match(
                data => CreatedAtAction(nameof(InstructorGet), new { id = data.Id }, data),
                error => this.HandleServiceError(error)
            );
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
            var result = await instructorService.GetInstructorByIdAsync(id, nameof(InstructorGet), ct);
            return result.Match(
                data => Ok(data),
                error => this.HandleServiceError(error)
            );
        }
    }
}