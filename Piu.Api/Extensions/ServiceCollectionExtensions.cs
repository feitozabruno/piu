using Piu.Api.Extensions;
using Piu.Api.Features.Auth;
using Piu.Api.Infrastructure.Persistence;
using Piu.Api.Middlewares;

namespace Piu.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPiuServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddGlobalExceptionHandling();
        services.AddDatabase(configuration);
        services.AddAuth();
        services.AddFeatureHandlers();
        services.AddValidation();
        services.AddWebCors();

        return services;
    }
}
