namespace Piu.Api.Domain;

public sealed record DomainError(string Code, string Message);

public sealed class DomainException(DomainError error) : Exception(error.Message)
{
    public DomainError Error { get; } = error;
}
