using OnlineCourse.Dtos;

namespace OnlineCourse.Services.IServices
{
    public interface IInstructorService
    {
        Task<InstructorDto> CreateInstructorAsync(InstructorCreationDto instructorCreationDto, CancellationToken ct = default);
        Task<InstructorDto?> GetInstructorByIdAsync(Guid id, CancellationToken ct = default);
    }
}