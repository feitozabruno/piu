namespace Piu.Api.Features.Admin.CriarIncubatorio;

public static class CriarIncubatorioEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/incubatorios", CriarAsync)
            .RequireAuthorization(Features.Auth.Usuario.Administrador)
            .WithName("CriarIncubatorioComGerente");
    }

    private static async Task<IResult> CriarAsync(
        CriarIncubatorioRequest request,
        CriarIncubatorioHandler handler,
        CancellationToken cancellationToken)
    {
        var (incubatorio, gerente) = await handler.HandleAsync(request, cancellationToken);
        return Results.Created(
            $"/api/v1/incubatorios/{incubatorio.Identificador}",
            new CriarIncubatorioResponse(
                incubatorio.Identificador,
                incubatorio.Nome,
                incubatorio.Sigla,
                gerente.UserName!,
                gerente.Nome));
    }
}
