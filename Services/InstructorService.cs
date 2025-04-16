using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineCourse.Dtos;
using OnlineCourse.Entities;
using OnlineCourse.Exceptions;
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
            CancellationToken ct = default) 
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                var userMapped = _mapper.Map<User>(instructorCreationDto);
                var userResult = await _userManager.CreateAsync(userMapped, instructorCreationDto.Password);
                if (!userResult.Succeeded)
                {
                    string errorTitle = "Error al crear el usuario.";
                    throw new UserCreationException(userResult.Errors, errorTitle);
                }
                var instructorMapped = _mapper.Map<Instructor>(instructorCreationDto);
                instructorMapped.User = userMapped;

                await _unitOfWork.Instructors.AddAsync(instructorMapped, ct);
                await _unitOfWork.CompleteAsync(ct);
                await transaction.CommitAsync(ct);

                return _mapper.Map<InstructorDto>(instructorMapped);
            }
            catch (Exception) 
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
        public async Task<InstructorDto?> GetInstructorByIdAsync(
            Guid id, 
            CancellationToken ct = default)
        {
            var instructor = await _unitOfWork.Instructors.GetByIdAsync(id, ct);
            if (instructor == null) return null;
            return _mapper.Map<InstructorDto>(instructor);
        }
    }
}   