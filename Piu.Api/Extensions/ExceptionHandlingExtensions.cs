using Piu.Api.Middleware;

namespace Piu.Api.Extensions;

public static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddGlobalExceptionHandling(
        this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                if (context.ProblemDetails is HttpValidationProblemDetails)
                {
                    context.ProblemDetails.Title = "Um ou mais erros de validação ocorreram.";
                }
            };
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
