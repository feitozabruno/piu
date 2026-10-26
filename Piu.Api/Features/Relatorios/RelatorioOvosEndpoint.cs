using System.Security.Claims;

namespace Piu.Api.Features.Relatorios;

public static class RelatorioOvosEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/ovos", GerarAsync)
            .RequireAuthorization()
            .WithName("GerarRelatorioOvos")
            .WithSummary("Gera relatórios diários de ovos recebidos.");
    }

    private static async Task<IResult> GerarAsync(
        DateOnly dataInicial,
        DateOnly dataFinal,
        ClaimsPrincipal usuarioAutenticado,
        RelatorioOvosService service,
        CancellationToken cancellationToken)
    {
        var resultado = await service.GerarAsync(
            dataInicial,
            dataFinal,
            usuarioAutenticado,
            cancellationToken);

        if (resultado.Arquivos.Count == 0)
            return Results.NoContent();

        if (resultado.Arquivos.Count == 1)
        {
            var arquivo = resultado.Arquivos[0];
            return Results.File(arquivo.Conteudo, "application/pdf", arquivo.Nome);
        }

        return Results.File(
            resultado.ComoZip(),
            "application/zip",
            $"controle-ovos-{dataInicial:yyyy-MM-dd}-a-{dataFinal:yyyy-MM-dd}.zip");
    }
}
