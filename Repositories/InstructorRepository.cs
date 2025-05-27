using Microsoft.EntityFrameworkCore;
using OnlineCourse.Dtos;
using OnlineCourse.Entities;
using OnlineCourse.Repositories.IRepositories;

namespace OnlineCourse.Repositories
{
    public class InstructorRepository(ApplicationDbContext context) : IInstructorRepository
    {
        public async Task AddAsync(Instructor instructor, CancellationToken ct = default)
        {
            await context.Instructors.AddAsync(instructor, ct);
        }
        public async Task SaveChangeAsync( CancellationToken ct)
        {
            await context.SaveChangesAsync(ct);
        }
        public async Task<InstructorDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await context.Instructors
                .Where(i =>i.Id == id)
                .Select(i => new InstructorDto
                {
                 Id = i.Id,
                 Email = i.User.Email!,
                 Biography = i.Biography,
                 WebSiteUrl = i.WebSiteUrl
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}