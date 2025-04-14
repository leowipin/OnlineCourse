using Microsoft.EntityFrameworkCore.Storage;
using OnlineCourse.Repositories.IRepositories;

namespace OnlineCourse.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IInstructorRepository Instructors { get; }
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}