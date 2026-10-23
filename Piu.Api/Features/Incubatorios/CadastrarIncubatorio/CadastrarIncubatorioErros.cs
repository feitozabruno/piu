using Piu.Api.Exceptions;

namespace Piu.Api.Features.Incubatorios.CadastrarIncubatorio;

public static class CadastrarIncubatorioErros
{
    public const string IdentificadorObrigatorio = "O identificador do incubatório é obrigatório.";
    public const string IdentificadorCurto = "O identificador do incubatório precisa ter pelo menos {1} caracteres.";
    public const string IdentificadorMuitoGrande = "O identificador do incubatório pode ter no máximo {1} caracteres.";
    public static readonly ApplicationError ErroIdentificadorEmUso =
        new("IDENTIFICADOR_EM_USO", "Este identificador do incubatório já está em uso.", StatusCodes.Status409Conflict);

    public const string NomeObrigatorio = "O nome do incubatório é obrigatório.";
    public const string NomeCurto = "O nome do incubatório precisa ter pelo menos {1} caracteres.";
    public const string NomeMuitoGrande = "O nome do incubatório pode ter no máximo {1} caracteres.";

    public const string SiglaObrigatoria = "A sigla do incubatório é obrigatória.";
    public const string SiglaCurta = "A sigla do incubatório precisa ter pelo menos {1} caracteres.";
    public const string SiglaMuitoGrande = "A sigla do incubatório pode ter no máximo {1} caracteres.";
}
