using Piu.Api.Features.Lotes.CadastrarLote;
using Piu.Api.Features.Lotes.ListarLotes;

namespace Piu.Api.Features.Lotes;

public static class LoteEndpoints
{
    public static IEndpointRouteBuilder MapLotes(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v1/lotes").WithTags("Lotes");
        CadastrarLoteEndpoint.Map(grupo);
        ListarLotesEndpoint.Map(grupo);
        return app;
    }
}
