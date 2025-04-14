using Microsoft.EntityFrameworkCore.Storage;
using OnlineCourse.Repositories;
using OnlineCourse.Repositories.IRepositories;

namespace OnlineCourse.UnitOfWork
{
    public class UnitOfWork (ApplicationDbContext context): IUnitOfWork
    {
        // Context
        private readonly ApplicationDbContext _context = context;
        // Repositories
        private IInstructorRepository? _instructorRepository;
        public IInstructorRepository Instructors =>
            _instructorRepository ??= new InstructorRepository(_context);
        // Functions
        public async Task<int> CompleteAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
        // Transaction
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            return await _context.Database.BeginTransactionAsync(ct);
        }
        // Free up
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}