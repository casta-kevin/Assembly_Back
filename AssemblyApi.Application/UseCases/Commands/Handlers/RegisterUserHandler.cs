using AssemblyApi.Application.DTOs;
using AssemblyApi.Application.Repositories;
using AssemblyApi.Application.Services;
using AssemblyApi.Domain.Entities;
using AssemblyApi.Domain.Exceptions;
using MediatR;

namespace AssemblyApi.Application.UseCases.Commands.Handlers;

public class RegisterUserHandler : IRequestHandler<RegisterUser, RegisterUserResponseDto>
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

    public async Task<RegisterUserResponseDto> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        var data = request.Data;

        if (string.IsNullOrWhiteSpace(data.Username))
            throw new DomainException("El nombre de usuario es requerido");

        if (string.IsNullOrWhiteSpace(data.Password))
            throw new DomainException("La contraseña es requerida");

        if (data.Password.Length < 6)
            throw new DomainException("La contraseña debe tener al menos 6 caracteres");

        if (data.PropertyId == Guid.Empty)
            throw new DomainException("La propiedad es requerida");

        var usernameExists = await _userRepository.ExistsUsernameAsync(data.Username, cancellationToken);
        if (usernameExists)
            throw new DomainException("El nombre de usuario ya está registrado");

        if (!string.IsNullOrWhiteSpace(data.Email))
        {
            var emailExists = await _userRepository.ExistsEmailAsync(data.Email, cancellationToken);
            if (emailExists)
                throw new DomainException("El email ya está registrado");
        }

        var passwordHash = _passwordHashService.HashPassword(data.Password);

        var user = new User(data.Username, passwordHash, data.PropertyId, data.Email);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterUserResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email ?? string.Empty,
            PropertyId = user.PropertyId
        };
    }
}
