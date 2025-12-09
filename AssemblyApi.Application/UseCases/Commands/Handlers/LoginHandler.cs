using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using AssemblyApi.Domain.Exceptions;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class LoginHandler : IRequestHandler<Login, LoginResponseDto>
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

    public async Task<LoginResponseDto> Handle(Login request, CancellationToken cancellationToken)
    {
        var data = request.Data;

        var user = await _userRepository.GetByUsernameAsync(data.Username, cancellationToken);
        
        if (user == null)
            throw new DomainException("Usuario o contraseña incorrectos");

        if (!user.IsActive)
            throw new DomainException("El usuario está desactivado");

        var isValidPassword = _passwordHashService.VerifyPassword(data.Password, user.PasswordHash);
        if (!isValidPassword)
            throw new DomainException("Usuario o contraseña incorrectos");

        var token = _jwtTokenService.GenerateToken(user.Id, user.PropertyId, user.Username);

        user.RecordLogin();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            PropertyId = user.PropertyId,
            Username = user.Username
        };
    }
}
