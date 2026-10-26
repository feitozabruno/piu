using Piu.Api.Features.Admin.CriarFuncionario;
using Piu.Api.Features.Admin.CriarIncubatorio;
using Piu.Api.Features.Admin.ListarIncubatorios;

namespace Piu.Api.Features.Admin;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdmin(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v1/admin").WithTags("Administração");
        CriarIncubatorioEndpoint.Map(grupo);
        ListarIncubatoriosEndpoint.Map(grupo);

        var gerencia = app.MapGroup("/api/v1/gerente").WithTags("Gerência");
        CriarFuncionarioEndpoint.Map(gerencia);
        return app;
    }
}
