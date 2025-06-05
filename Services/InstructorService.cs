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
        //public async Task<Result<InstructorDto>> CreateInstructorAsync(
        //    InstructorCreationDto instructorCreationDto,
        //    string? endpointInfo = null,
        //    CancellationToken ct = default) 
        //{
        //    await using var transaction = await unitOfWork.BeginTransactionAsync(ct);

        //    var userMapped = mapper.Map<User>(instructorCreationDto);
        //    var userResult = await userManager.CreateAsync(userMapped, instructorCreationDto.Password);
        //    if (!userResult.Succeeded)
        //    {
        //        transaction.Rollback();
        //        var userIdentityError = new UserIdentityErrorWrapper(identityErrors: userResult.Errors);
        //        logger.LogServiceEvent(userIdentityError, LogLevel.Information, endpointInfo);
        //        return Result<InstructorDto>.Failure(userIdentityError);
        //    }

        //    var roleResult = await userManager.AddToRoleAsync(userMapped, AppRoles.Instructor);
        //    if (!roleResult.Succeeded)
        //    {
        //        transaction.Rollback();
        //        var roleAssignErrorForLog = new RoleIdentityErrorWrapper(
        //            identityErrors: roleResult.Errors, AppRoles.Instructor);
        //        logger.LogServiceEvent(roleAssignErrorForLog, LogLevel.Error, endpointInfo);
        //        var roleAssignErrorForClient = new RoleIdentityErrorWrapper(roleResult.Errors);
        //        return Result<InstructorDto>.Failure(roleAssignErrorForClient);
        //    }   

        //    var instructorMapped = mapper.Map<Instructor>(instructorCreationDto);
        //    instructorMapped.User = userMapped;

        //    await unitOfWork.Instructors.AddAsync(instructorMapped, ct);
        //    await unitOfWork.CompleteAsync(ct);
        //    await transaction.CommitAsync(ct);

        //    return Result<InstructorDto>.Success(mapper.Map<InstructorDto>(instructorMapped));
        //}
        public async Task<Result<InstructorDto>> CreateInstructorAsync(
    InstructorCreationDto instructorCreationDto,
    string? endpointInfo = null,
    CancellationToken ct = default)
        {
            await using var transaction = await unitOfWork.BeginTransactionAsync(ct);

            var userMapped = mapper.Map<User>(instructorCreationDto);

            // PASO 1: Asegúrate de que el usuario exista y tenga el rol
            // Esto es solo para fines de prueba de la segunda guard clause
            var existingUser = await userManager.FindByEmailAsync(instructorCreationDto.Email);
            if (existingUser == null)
            {
                // Crear el usuario si no existe para esta prueba
                var creationResult = await userManager.CreateAsync(userMapped, instructorCreationDto.Password);
                if (!creationResult.Succeeded)
                {
                    // Manejar error de creación si es necesario para la prueba
                    await transaction.RollbackAsync(ct);
                    return Result<InstructorDto>.Failure(new UserIdentityErrorWrapper(creationResult.Errors));
                }
                existingUser = userMapped; // userMapped ahora tiene un ID
            }

            // Asignar el rol una vez (para que la segunda vez falle)
            var firstRoleAssignResult = await userManager.AddToRoleAsync(existingUser, AppRoles.Instructor);
            if (!firstRoleAssignResult.Succeeded)
            {
                // Si esto falla, la prueba de "UserAlreadyInRole" no es posible
                // o quizás el rol no existe, lo que también probaría la segunda guard clause
                await transaction.RollbackAsync(ct);
                var roleAssignErrorForLog = new RoleIdentityErrorWrapper(
                    identityErrors: firstRoleAssignResult.Errors, AppRoles.Instructor);
                logger.LogServiceEvent(roleAssignErrorForLog, LogLevel.Error, endpointInfo);
                return Result<InstructorDto>.Failure(new RoleIdentityErrorWrapper(firstRoleAssignResult.Errors));
            }
            // En este punto, el usuario existe y tiene el rol.
            // ¡OJO! Si el rol ya estaba asignado, firstRoleAssignResult.Succeeded sería false y ya habríamos retornado.
            // Para que UserAlreadyInRole ocurra, necesitas que la PRIMERA asignación tenga éxito.

            // PASO 2: Intentar asignar el rol OTRA VEZ para forzar "UserAlreadyInRole"
            // Esta es la llamada que quieres que falle y sea atrapada por tu segunda guard clause
            var roleResult = await userManager.AddToRoleAsync(existingUser, AppRoles.Instructor); // Usar existingUser
            if (!roleResult.Succeeded)
            {
                // AQUÍ DEBERÍAS OBTENER "UserAlreadyInRole" SI TODO LO ANTERIOR FUNCIONÓ
                await transaction.RollbackAsync(ct);
                var roleAssignErrorForLog = new RoleIdentityErrorWrapper(
                    identityErrors: roleResult.Errors, AppRoles.Instructor);
                logger.LogServiceEvent(roleAssignErrorForLog, LogLevel.Error, endpointInfo);
                var roleAssignErrorForClient = new RoleIdentityErrorWrapper(roleResult.Errors);
                return Result<InstructorDto>.Failure(roleAssignErrorForClient);
            }

            // El resto de tu lógica
            var instructorMappedForDb = mapper.Map<Instructor>(instructorCreationDto);
            instructorMappedForDb.User = existingUser; // Usar el usuario que sabemos que existe

            await unitOfWork.Instructors.AddAsync(instructorMappedForDb, ct);
            await unitOfWork.CompleteAsync(ct);
            await transaction.CommitAsync(ct);

            return Result<InstructorDto>.Success(mapper.Map<InstructorDto>(instructorMappedForDb));
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