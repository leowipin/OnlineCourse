using Microsoft.EntityFrameworkCore;
using OnlineCourse.Entities;
using OnlineCourse.Repositories.IRepositories;

namespace OnlineCourse.Repositories
{
    public class InstructorRepository(ApplicationDbContext context) : IInstructorRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task AddAsync(Instructor instructor, CancellationToken cancellationToken = default)
        {
            await _context.Instructors.AddAsync(instructor);
        }
        public async Task SaveChangeAsync( CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<Instructor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Instructors
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }
    }
}