namespace Piu.Api.Features.Relatorios;

public static class RelatorioEndpoints
{
    public static IEndpointRouteBuilder MapRelatorios(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v1/relatorios").WithTags("Relatórios");
        RelatorioOvosEndpoint.Map(grupo);
        return app;
    }
}
