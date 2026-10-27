using System.Security.Claims;
using Piu.Api.Features.Ovos.ReceberOvo;

namespace Piu.Api.Features.Ovos.ConsultarOvo;

public static class ConsultarOvoEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/", ListarAsync)
            .RequireAuthorization();

        app.MapGet("/{id:guid}", ConsultarAsync)
            .RequireAuthorization();
    }

    private static async Task<IResult> ListarAsync(
        DateOnly? dataInicial,
        DateOnly? dataFinal,
        ClaimsPrincipal usuarioAutenticado,
        ConsultarOvoHandler handler,
        CancellationToken cancellationToken)
    {
        var ovos = await handler.ListarAsync(
            dataInicial,
            dataFinal,
            usuarioAutenticado,
            cancellationToken);

        return Results.Ok(ovos.Select(ReceberOvoResponse.From));
    }

    private static async Task<IResult> ConsultarAsync(
        Guid id,
        ClaimsPrincipal usuarioAutenticado,
        ConsultarOvoHandler handler)
    {
        var ovo = await handler.HandleAsync(id, usuarioAutenticado);
        return Results.Ok(ReceberOvoResponse.From(ovo));
    }
}
