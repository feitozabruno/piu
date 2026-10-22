using System.Security.Claims;

namespace Piu.Api.Features.Lotes.ListarLotes;

public static class ListarLotesEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/", ListarAsync)
            .RequireAuthorization();
    }

    private static async Task<IResult> ListarAsync(
        ClaimsPrincipal usuarioAutenticado,
        ListarLotesHandler handler)
    {
        var lotes = await handler.HandleAsync(usuarioAutenticado);
        return Results.Ok(lotes);
    }
}
