namespace Piu.Api.Features.Admin.CriarIncubatorio;

public sealed record CriarIncubatorioResponse(
    string Identificador,
    string Nome,
    string Sigla,
    string UsuarioGerente,
    string NomeGerente);
