using Microsoft.EntityFrameworkCore.Storage;
using OnlineCourse.Repositories;
using OnlineCourse.Repositories.IRepositories;

namespace OnlineCourse.UnitOfWork
{
    public class UnitOfWork (ApplicationDbContext context): IUnitOfWork
    {
        // Repositories
        private IInstructorRepository? _instructorRepository;
        public IInstructorRepository Instructors =>
            _instructorRepository ??= new InstructorRepository(context);
        // Functions
        public async Task<int> CompleteAsync(CancellationToken ct = default)
        {
            return await context.SaveChangesAsync(ct);
        }
        // Transaction
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            return await context.Database.BeginTransactionAsync(ct);
        }
        // Free up
        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}