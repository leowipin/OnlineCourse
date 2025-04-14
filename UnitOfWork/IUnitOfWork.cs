using Microsoft.EntityFrameworkCore.Storage;
using OnlineCourse.Repositories.IRepositories;

namespace OnlineCourse.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IInstructorRepository Instructors { get; }
        Task<int> CompleteAsync(CancellationToken ct = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
    }
}