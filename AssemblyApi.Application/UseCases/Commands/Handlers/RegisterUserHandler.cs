using AssemblyApi.Application.Constants;
using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Domain.Exceptions;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUser, ApiResponse<RegisterUserResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<RegisterUserResponseDto>> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        try
        {
            var data = request.Data;

            if (string.IsNullOrWhiteSpace(data.Username))
                return ApiResponse<RegisterUserResponseDto>.FailureResponse("El nombre de usuario es requerido");

            if (string.IsNullOrWhiteSpace(data.Password))
                return ApiResponse<RegisterUserResponseDto>.FailureResponse("La contrasena es requerida");

            if (data.Password.Length < 6)
                return ApiResponse<RegisterUserResponseDto>.FailureResponse("La contrasena debe tener al menos 6 caracteres");

            if (data.PropertyId == Guid.Empty)
                return ApiResponse<RegisterUserResponseDto>.FailureResponse("La propiedad es requerida");

            var roleId = string.IsNullOrWhiteSpace(data.RoleId)
                ? RoleIds.Resident
                : data.RoleId.Trim().ToUpperInvariant();

            if (roleId.Length > 10)
                return ApiResponse<RegisterUserResponseDto>.FailureResponse("El identificador del rol no puede superar 10 caracteres");

            var usernameExists = await _userRepository.ExistsUsernameAsync(data.Username, cancellationToken);
            if (usernameExists)
                return ApiResponse<RegisterUserResponseDto>.FailureResponse("El nombre de usuario ya esta registrado");

            if (!string.IsNullOrWhiteSpace(data.Email))
            {
                var emailExists = await _userRepository.ExistsEmailAsync(data.Email, cancellationToken);
                if (emailExists)
                    return ApiResponse<RegisterUserResponseDto>.FailureResponse("El email ya esta registrado");
            }

            var passwordHash = _passwordHashService.HashPassword(data.Password);

            var user = new User(data.Username, passwordHash, data.PropertyId, data.Email, roleId);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new RegisterUserResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email ?? string.Empty,
                PropertyId = user.PropertyId,
                RoleId = user.RoleId
            };

            return ApiResponse<RegisterUserResponseDto>.SuccessResponse(response, "Usuario registrado correctamente");
        }
        catch (DomainException dex)
        {
            return ApiResponse<RegisterUserResponseDto>.FailureResponse(dex.Message);
        }
        catch (Exception ex)
        {
            return ApiResponse<RegisterUserResponseDto>.FailureResponse("Error al registrar el usuario", new[] { ex.Message });
        }
    }
}
