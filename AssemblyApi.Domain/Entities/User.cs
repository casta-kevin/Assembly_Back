using AssemblyApi.Domain.Common;
using AssemblyApi.Domain.Exceptions;

namespace AssemblyApi.Domain.Entities;

public class User : Entity
{
    public string Username { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;
    public Guid PropertyId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }

    private User() { }

    public User(string username, string passwordHash, Guid propertyId, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("El nombre de usuario es requerido");

        if (username.Length < 3)
            throw new DomainException("El nombre de usuario debe tener al menos 3 caracteres");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("El hash de contraseña es requerido");

        if (propertyId == Guid.Empty)
            throw new DomainException("La propiedad es requerida");

        Username = username;
        PasswordHash = passwordHash;
        PropertyId = propertyId;
        Email = email;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(string email)
    {
        if (!string.IsNullOrWhiteSpace(email) && !email.Contains('@'))
            throw new DomainException("El email debe tener un formato válido");

        Email = email;
    }

    public void UpdatePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("El hash de contraseña no puede estar vacío");

        PasswordHash = passwordHash;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new DomainException("El usuario ya está desactivado");

        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive)
            throw new DomainException("El usuario ya está activo");

        IsActive = true;
    }

    public void RecordLogin()
    {
        if (!IsActive)
            throw new DomainException("No se puede registrar login de un usuario inactivo");

        LastLoginAt = DateTime.UtcNow;
    }
}
