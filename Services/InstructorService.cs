using AutoMapper;
using Microsoft.AspNetCore.Identity;
using OnlineCourse.Data.Constants;
using OnlineCourse.Dtos;
using OnlineCourse.Entities;
using OnlineCourse.Extensions.Logging;
using OnlineCourse.Primitives;
using OnlineCourse.Services.IServices;
using OnlineCourse.UnitOfWork;

namespace OnlineCourse.Services
{
    public class InstructorService(IMapper mapper, UserManager<User> userManager,
        IUnitOfWork unitOfWork, ILogger<InstructorService> logger) : IInstructorService
    {
        public async Task<Result<InstructorDto>> CreateInstructorAsync(
            InstructorCreationDto instructorCreationDto,
            string? endpointInfo = null,
            CancellationToken ct = default) 
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync(ct);
            
            var userMapped = mapper.Map<User>(instructorCreationDto);
            var userResult = await userManager.CreateAsync(userMapped, instructorCreationDto.Password);
            if (!userResult.Succeeded)
            {
                transaction.Rollback();
                var userIdentityError = new UserIdentityErrorWrapper(identityErrors: userResult.Errors);
                logger.LogServiceEvent(userIdentityError, LogLevel.Information, endpointInfo);
                return Result<InstructorDto>.Failure(userIdentityError);
            }
                
            var roleResult = await userManager.AddToRoleAsync(userMapped, AppRoles.Instructor);
            if (!roleResult.Succeeded)
            {
                transaction.Rollback();
                var roleAssignError = new RoleIdentityErrorWrapper(
                    identityErrors: roleResult.Errors, AppRoles.Instructor);
                logger.LogServiceEvent(roleAssignError, LogLevel.Error, endpointInfo);
                return Result<InstructorDto>.Failure(roleAssignError);
            }
                
            var instructorMapped = mapper.Map<Instructor>(instructorCreationDto);
            instructorMapped.User = userMapped;

            await unitOfWork.Instructors.AddAsync(instructorMapped, ct);
            await unitOfWork.CompleteAsync(ct);
            await transaction.CommitAsync(ct);

            return Result<InstructorDto>.Success(mapper.Map<InstructorDto>(instructorMapped));
        }
        public async Task<Result<InstructorDto>> GetInstructorByIdAsync(
            Guid id,
            string? endpointInfo = null,
            CancellationToken ct = default)
        {
            var instructorDto = await unitOfWork.Instructors.GetByIdAsync(id, ct);
            if (instructorDto is null)
            {
                var notFoundError = new NotFoundError(resourceName: nameof(Instructor), id: id);
                logger.LogServiceEvent(notFoundError, LogLevel.Error, endpointInfo);
                return Result<InstructorDto>.Failure(notFoundError);
            }
            return Result<InstructorDto>.Success(instructorDto);
        }
    }
}   