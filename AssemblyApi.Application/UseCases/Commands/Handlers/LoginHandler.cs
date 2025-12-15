using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using AssemblyApi.Domain.Exceptions;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class LoginHandler : IRequestHandler<Login, ApiResponse<LoginResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginHandler(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHashService passwordHashService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHashService = passwordHashService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<LoginResponseDto>> Handle(Login request, CancellationToken cancellationToken)
    {
        try
        {
            var data = request.Data;

            if (string.IsNullOrWhiteSpace(data.Username))
                return ApiResponse<LoginResponseDto>.FailureResponse("Usuario o correo es requerido");

            var identifier = data.Username.Trim();

            var user = await _userRepository.GetByUsernameAsync(identifier, cancellationToken)
                ?? await _userRepository.GetByEmailAsync(identifier, cancellationToken);

            if (user == null)
                return ApiResponse<LoginResponseDto>.FailureResponse("Usuario/correo o contrasena incorrectos");

            if (!user.IsActive)
                return ApiResponse<LoginResponseDto>.FailureResponse("El usuario esta desactivado");

            var isValidPassword = _passwordHashService.VerifyPassword(data.Password, user.PasswordHash);
            if (!isValidPassword)
                return ApiResponse<LoginResponseDto>.FailureResponse("Usuario/correo o contrasena incorrectos");

            var token = _jwtTokenService.GenerateToken(user.Id, user.PropertyId, user.Username, user.RoleId);

            user.RecordLogin();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                PropertyId = user.PropertyId,
                Username = user.Username,
                RoleId = user.RoleId
            };

            return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login exitoso");
        }
        catch (DomainException dex)
        {
            return ApiResponse<LoginResponseDto>.FailureResponse(dex.Message);
        }
        catch (Exception ex)
        {
            return ApiResponse<LoginResponseDto>.FailureResponse("Error en el proceso de login", new[] { ex.Message });
        }
    }
}
