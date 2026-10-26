namespace Piu.Api.Features.Relatorios;

public static class RelatorioOvosServiceCollectionExtensions
{
    public static IServiceCollection AddOvoReport(this IServiceCollection services)
    {
        services.AddOptions<RelatorioOvosOptions>();
        return services;
    }
}
