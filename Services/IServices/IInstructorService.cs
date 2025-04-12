using OnlineCourse.Dtos;

namespace OnlineCourse.Services.IServices
{
    public interface IInstructorService
    {
        Task<InstructorDto> CreateInstructorAsync(InstructorCreationDto instructorCreationDto);
        Task<InstructorDto?> GetInstructorByIdAsync(Guid id);
    }
}