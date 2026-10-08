namespace Piu.Api.Application;

public sealed record ApplicationError(string Code, string Message);

public sealed class ApplicationErrorException(ApplicationError error) : Exception(error.Message)
{
    public ApplicationError Error { get; } = error;
}
