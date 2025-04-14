using OnlineCourse.Dtos;

namespace OnlineCourse.Services.IServices
{
    public interface IInstructorService
    {
        Task<InstructorDto> CreateInstructorAsync(InstructorCreationDto instructorCreationDto, CancellationToken cancellationToken = default);
        Task<InstructorDto?> GetInstructorByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}