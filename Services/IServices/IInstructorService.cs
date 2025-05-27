using OnlineCourse.Dtos;
using OnlineCourse.Primitives;

namespace OnlineCourse.Services.IServices
{
    public interface IInstructorService
    {
        Task<Result<InstructorDto>> CreateInstructorAsync(
            InstructorCreationDto instructorCreationDto,
            string? endpointInfo = null,
            CancellationToken ct = default);
        Task<Result<InstructorDto>> GetInstructorByIdAsync(
            Guid id,
            string? endpointInfo = null,
            CancellationToken ct = default);
    }
}