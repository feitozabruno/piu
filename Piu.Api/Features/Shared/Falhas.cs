using Microsoft.AspNetCore.Http.HttpResults;

namespace Piu.Api.Features.Shared;

public static class Falhas
{
    public static ValidationProblem Validacao(IReadOnlyDictionary<string, string[]>? erros) =>
        TypedResults.ValidationProblem(
            erros?.ToDictionary(e => e.Key, e => e.Value) ?? [],
            title: "Um ou mais erros de validação ocorreram.");
}
