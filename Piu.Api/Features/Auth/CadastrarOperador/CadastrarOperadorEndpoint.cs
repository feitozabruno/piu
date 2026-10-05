using Microsoft.AspNetCore.Http.HttpResults;

namespace Piu.Api.Features.Auth.CadastrarOperador;

using Resultado = Results<
    Ok<CadastrarOperadorResponse>,
    ValidationProblem>;

public static class CadastrarOperadorEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/cadastrar", CriarAsync);
    }

    private static async Task<Resultado> CriarAsync(
        CadastrarOperadorRequest request,
        CadastrarOperadorService service)
    {
        var resposta = await service.CriarAsync(request);

        if (resposta.Sucesso)
            return TypedResults.Ok(resposta);

        return Falhas.Validacao(resposta.Erros);
    }
};

public static class Falhas
{
    public static ValidationProblem Validacao(IReadOnlyDictionary<string, string[]>? erros) =>
        TypedResults.ValidationProblem(
            erros?.ToDictionary(e => e.Key, e => e.Value) ?? [],
            title: "Um ou mais erros de validação ocorreram.");
}
