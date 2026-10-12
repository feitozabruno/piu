namespace Piu.Api.Features.Incubatorios.CadastrarIncubatorio;

public static class CadastrarIncubatorioEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/", CriarAsync);
    }

    private static async Task<IResult> CriarAsync(
        CadastrarIncubatorioRequest request,
        CadastrarIncubatorioHandler handler)
    {
        var incubatorio = await handler.HandleAsync(request);

        var response = new CadastrarIncubatorioResponse(
            Identificador: incubatorio.Identificador,
            Nome: incubatorio.Nome,
            Sigla: incubatorio.Sigla,
            Ativo: incubatorio.Ativo,
            DataCadastro: incubatorio.DataCadastro
        );

        return Results.Created(
            $"/api/v1/incubatorios/{incubatorio.Identificador}",
            response);
    }
}
