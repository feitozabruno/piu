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
        RelatorioOvosHandler handler,
        CancellationToken cancellationToken)
    {
        var resultado = await handler.GerarAsync(
            dataInicial,
            dataFinal,
            usuarioAutenticado,
            cancellationToken);

        if (resultado.Conteudo is null)
            return Results.NoContent();

        return Results.File(
            resultado.Conteudo,
            "application/pdf",
            $"controle-ovos-{dataInicial:yyyy-MM-dd}-a-{dataFinal:yyyy-MM-dd}.pdf");
    }
}
