namespace Piu.Api.Features.Lotes.CadastrarLote;

public static class CadastrarLoteEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", CriarAsync);
    }

    private static async Task<IResult> CriarAsync(
        CadastrarLoteRequest request,
        CadastrarLoteHandler handler)
    {
        var novoLote = await handler.HandleAsync(request);

        var response = new CadastrarLoteResponse(
            Id: novoLote.Id,
            Nome: novoLote.Nome,
            DataNascimento: novoLote.DataNascimento
        );

        return Results.Created($"/api/v1/lotes/{novoLote.Id}", response);
    }
};
