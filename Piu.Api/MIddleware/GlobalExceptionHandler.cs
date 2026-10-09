using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Piu.Api.Application;
using Piu.Api.Domain;

namespace Piu.Api.Middleware;

public sealed partial class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private const string _internalErrorCode = "ERRO_INTERNO";

    private const string _internalErrorMessage =
        "Ocorreu um erro interno. Tente novamente mais tarde.";

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var request = httpContext.Request;

        if (exception is OperationCanceledException
            && httpContext.RequestAborted.IsCancellationRequested)
        {
            LogRequestCancelled(
                logger,
                request.Method,
                request.Path
            );

            return true;
        }

        var (statusCode, code, message) = MapException(exception);

        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            LogServerError(
                logger,
                exception,
                request.Method,
                request.Path,
                statusCode,
                code
            );
        }
        else
        {
            LogClientError(
                logger,
                exception,
                request.Method,
                request.Path,
                statusCode,
                code
            );
        }

        httpContext.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode >= StatusCodes.Status500InternalServerError
                ? "Erro interno do servidor"
                : "Não foi possível processar a requisição",
            Detail = message,
            Instance = request.Path
        };

        problem.Extensions["code"] = code;

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        };

        var written = await problemDetailsService.TryWriteAsync(
            problemDetailsContext
        );

        if (written)
            return true;

        if (httpContext.Response.HasStarted)
            return true;

        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problem,
            cancellationToken: cancellationToken
        );

        return true;
    }

    private static (int StatusCode, string Code, string Message) MapException(Exception exception) =>
        exception switch
        {
            ApplicationErrorException ex => (ex.Error.StatusCode, ex.Error.Code, ex.Error.Message),
            DomainException ex => (StatusCodes.Status400BadRequest, ex.Error.Code, ex.Error.Message),
            _ => (StatusCodes.Status500InternalServerError, _internalErrorCode, _internalErrorMessage)
        };

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Requisição cancelada pelo cliente: {Method} {Path}")]
    private static partial void LogRequestCancelled(
        ILogger logger,
        string method,
        PathString path);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Erro ao processar {Method} {Path}. Status: {StatusCode}, Código: {Code}")]
    private static partial void LogClientError(
        ILogger logger,
        Exception exception,
        string method,
        PathString path,
        int statusCode,
        string code);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Erro ao processar {Method} {Path}. Status: {StatusCode}, Código: {Code}")]
    private static partial void LogServerError(
        ILogger logger,
        Exception exception,
        string method,
        PathString path,
        int statusCode,
        string code);
}
