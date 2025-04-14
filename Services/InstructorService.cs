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
using OnlineCourse.UnitOfWork;

namespace OnlineCourse.Services
{
    public class InstructorService(IMapper mapper, UserManager<User> userManager,
        IUnitOfWork unitOfWork) : IInstructorService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<InstructorDto> CreateInstructorAsync(
            InstructorCreationDto instructorCreationDto,
            CancellationToken cancellationToken = default) 
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            
            var userMapped = _mapper.Map<User>(instructorCreationDto);
            var userResult = await _userManager.CreateAsync(userMapped, instructorCreationDto.Password);
            if (userResult.Succeeded) throw new UserCreationException(userResult.Errors);
            
            var instructorMapped = _mapper.Map<Instructor>(instructorCreationDto);
            instructorMapped.User = userMapped;

            await _unitOfWork.Instructors.AddAsync(instructorMapped, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return _mapper.Map<InstructorDto>(instructorMapped);











            //using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            //var userMapped = _mapper.Map<User>(instructorCreationDto);
            //var userCreatedResult = await _userManager.CreateAsync(userMapped, instructorCreationDto.Password);
            //if (!userCreatedResult.Succeeded) throw new UserCreationException(userCreatedResult.Errors);

            //var instructorMapped = _mapper.Map<Instructor>(instructorCreationDto);
            //instructorMapped.User = userMapped;

            //await _instructorRepository.AddAsync(instructorMapped);
            //await _instructorRepository.SaveChangeAsync();

            //transaction.Complete();

            //return _mapper.Map<InstructorDto>(instructorMapped);
        }
        public async Task<InstructorDto?> GetInstructorByIdAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var instructor = await _unitOfWork.Instructors.GetByIdAsync(id);
            if (instructor == null) return null;
            return _mapper.Map<InstructorDto>(instructor);
        }
    }
}   