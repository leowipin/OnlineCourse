using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Dtos;
using OnlineCourse.Exceptions;
using OnlineCourse.Services.IServices;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class InstructorController(IInstructorService instructorService) : ControllerBase
    {
        private readonly IInstructorService _instructorService = instructorService;

        [HttpPost]
        [ProducesResponseType<InstructorDto>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorDto>> InstructorCreate(
            [FromBody] InstructorCreationDto instructorCreation,
            CancellationToken cancellationToken)
        {
            try
            {
                InstructorDto instructorDto = await _instructorService
                .CreateInstructorAsync(instructorCreation, cancellationToken);
                return CreatedAtAction(nameof(InstructorGet), new { id = instructorDto.Id }, instructorDto);
            }
            catch (UserCreationException uce)
            {
                return BadRequest(new { errors=uce.IdentityErrors, message=uce.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorDto>> InstructorGet(Guid id, CancellationToken cancellationToken)
        {
            var instructorDto = await _instructorService.GetInstructorByIdAsync(id, cancellationToken);
            if(instructorDto == null) return NotFound();
            return Ok(instructorDto);
        }
    }
}