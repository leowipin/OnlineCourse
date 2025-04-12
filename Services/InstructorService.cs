using System.Transactions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Dtos;
using OnlineCourse.Entities;
using OnlineCourse.Exceptions;
using OnlineCourse.Repositories.IRepositories;
using OnlineCourse.Services.IServices;

namespace OnlineCourse.Services
{
    public class InstructorService(IMapper mapper, UserManager<User> userManager,
        IInstructorRepository instructorRepository) : IInstructorService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IInstructorRepository _instructorRepository = instructorRepository;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<InstructorDto> CreateInstructorAsync(InstructorCreationDto instructorCreationDto) 
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var userMapped = _mapper.Map<User>(instructorCreationDto);
            var userCreatedResult = await _userManager.CreateAsync(userMapped, instructorCreationDto.Password);
            if (!userCreatedResult.Succeeded) throw new UserCreationException(userCreatedResult.Errors);
            
            var instructorMapped = _mapper.Map<Instructor>(instructorCreationDto);
            instructorMapped.User = userMapped;
            
            await _instructorRepository.AddAsync(instructorMapped);
            await _instructorRepository.SaveChangeAsync();
            
            transaction.Complete();

            return _mapper.Map<InstructorDto>(instructorMapped);
        }
        public async Task<InstructorDto?> GetInstructorByIdAsync(Guid id)
        {
            var instructorQuery = _instructorRepository
                .GetInstructorByIdQueryable(id)
                .ProjectTo<InstructorDto>((_mapper.ConfigurationProvider));
            return await instructorQuery.FirstOrDefaultAsync();
        }
    }
}