using Microsoft.AspNetCore.Mvc;
using OnlineCourse.Dtos;
using OnlineCourse.Services.IServices;

namespace OnlineCourse.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class InstructorController(IInstructorService instructorService) : ControllerBase
    {
        private readonly IInstructorService _instructorService = instructorService;

        [HttpPost]
        public async Task<ActionResult> InstructorCreate(
            [FromBody] InstructorCreationDto instructorCreation)
        {
            try
            {
                InstructorDto instructorDto = await _instructorService
                .CreateInstructorAsync(instructorCreation);
                return CreatedAtAction(nameof(InstructorGet), new { id = instructorDto.Id }, instructorDto);
            }
            //1 crear la excepcion de identity
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<InstructorDto>> InstructorGet(Guid id)
        {
            var instructorDto = await _instructorService.GetInstructorByIdAsync(id);
            if(instructorDto == null) return NotFound();
            return Ok(instructorDto);
        }
    }
}