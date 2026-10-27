using System.Security.Claims;

namespace Piu.Api.Features.Lotes.AtualizarLote;

public static class AtualizarLoteEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}", AtualizarAsync)
            .RequireAuthorization();
    }

    private static async Task<IResult> AtualizarAsync(
        Guid id,
        AtualizarLoteRequest request,
        ClaimsPrincipal usuarioAutenticado,
        AtualizarLoteHandler handler)
    {
        var lote = await handler.HandleAsync(id, request, usuarioAutenticado);

        return Results.Ok(new AtualizarLoteResponse(
            lote.Id,
            lote.Nome,
            lote.DataNascimento,
            lote.Ativo));
    }
}
