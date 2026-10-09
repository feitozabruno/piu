namespace Piu.Api.Application;

public sealed record ApplicationError(
    string Code,
    string Message,
    int StatusCode = StatusCodes.Status400BadRequest
);

public sealed class ApplicationErrorException(ApplicationError error) : Exception(error.Message)
{
    public ApplicationError Error { get; } = error;
}
