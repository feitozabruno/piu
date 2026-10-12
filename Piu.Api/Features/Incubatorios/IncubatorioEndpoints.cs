using Piu.Api.Features.Incubatorios.CadastrarIncubatorio;

namespace Piu.Api.Features.Incubatorios;

public static class IncubatorioEndpoints
{
    public static IEndpointRouteBuilder MapIncubatorios(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v1/incubatorios").WithTags("Incubatórios");
        CadastrarIncubatorioEndpoint.Map(grupo);
        return app;
    }
}
