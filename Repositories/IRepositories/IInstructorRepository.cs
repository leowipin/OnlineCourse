using OnlineCourse.Entities;

namespace OnlineCourse.Repositories.IRepositories
{
    public interface IInstructorRepository
    {
        Task AddAsync(Instructor instructor, CancellationToken cancellationToken = default);
        Task<Instructor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}