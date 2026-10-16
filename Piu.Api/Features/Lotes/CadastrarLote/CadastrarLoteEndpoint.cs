using System.Security.Claims;

namespace Piu.Api.Features.Lotes.CadastrarLote;

public static class CadastrarLoteEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", CriarAsync)
            .RequireAuthorization();
    }

    private static async Task<IResult> CriarAsync(
        CadastrarLoteRequest request,
        ClaimsPrincipal usuarioAutenticado,
        CadastrarLoteHandler handler)
    {
        var novoLote = await handler.HandleAsync(request, usuarioAutenticado);

        var response = new CadastrarLoteResponse(
            Id: novoLote.Id,
            Nome: novoLote.Nome,
            DataNascimento: novoLote.DataNascimento
        );

        return Results.Created($"/api/v1/lotes/{novoLote.Id}", response);
    }
};
