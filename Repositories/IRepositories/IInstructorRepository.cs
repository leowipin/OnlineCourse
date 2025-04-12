using OnlineCourse.Entities;

namespace OnlineCourse.Repositories.IRepositories
{
    public interface IInstructorRepository
    {
        Task AddAsync(Instructor instructor);
        Task SaveChangeAsync();
        IQueryable<Instructor> GetInstructorByIdQueryable(Guid id);
    }
}