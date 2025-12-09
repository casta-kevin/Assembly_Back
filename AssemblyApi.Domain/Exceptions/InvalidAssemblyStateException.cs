namespace AssemblyApi.Domain.Exceptions;

public class InvalidAssemblyStateException : DomainException
{
    public InvalidAssemblyStateException(string message) : base(message)
    {
    }
}
