using System.Security.Claims;

namespace Piu.Api.Features.Ovos.ReceberOvo;

public static class ReceberOvoEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", CriarAsync)
            .RequireAuthorization();
    }

    private static async Task<IResult> CriarAsync(
        ReceberOvoRequest request,
        ClaimsPrincipal usuarioAutenticado,
        ReceberOvoHandler handler)
    {
        var ovo = await handler.HandleAsync(request, usuarioAutenticado);

        return Results.Created(
            $"/api/v1/ovos/{ovo.Id}",
            ReceberOvoResponse.From(ovo));
    }
}
