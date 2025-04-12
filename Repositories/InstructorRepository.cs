using OnlineCourse.Entities;
using OnlineCourse.Repositories.IRepositories;

namespace OnlineCourse.Repositories
{
    public class InstructorRepository(ApplicationDbContext context) : IInstructorRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task AddAsync(Instructor instructor)
        {
            await _context.Instructors.AddAsync(instructor);
        }
        public async Task SaveChangeAsync( CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        public IQueryable<Instructor> GetInstructorByIdQueryable(Guid id)
        {
            return _context.Instructors.Where(i=>i.Id == id).AsQueryable();
        }
    }
}