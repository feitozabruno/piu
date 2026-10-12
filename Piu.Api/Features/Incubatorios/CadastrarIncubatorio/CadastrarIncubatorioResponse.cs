namespace Piu.Api.Features.Incubatorios.CadastrarIncubatorio;

public sealed record CadastrarIncubatorioResponse(
    string Identificador,
    string Nome,
    string Sigla,
    bool Ativo,
    DateTime DataCadastro
);
