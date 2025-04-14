using OnlineCourse.Entities;

namespace OnlineCourse.Repositories.IRepositories
{
    public interface IInstructorRepository
    {
        Task AddAsync(Instructor instructor, CancellationToken ct = default);
        Task<Instructor?> GetByIdAsync(Guid id, CancellationToken ct = default);
    }
}