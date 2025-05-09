using OnlineCourse.Dtos;
using OnlineCourse.Primitives;

namespace OnlineCourse.Services.IServices
{
    public interface IInstructorService
    {
        Task<Result<InstructorDto>> CreateInstructorAsync(InstructorCreationDto instructorCreationDto, CancellationToken ct = default);
        Task<Result<InstructorDto>> GetInstructorByIdAsync(Guid id, CancellationToken ct = default);
    }
}