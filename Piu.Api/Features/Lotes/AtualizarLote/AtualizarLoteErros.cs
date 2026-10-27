using Piu.Api.Exceptions;

namespace Piu.Api.Features.Lotes.AtualizarLote;

public static class AtualizarLoteErros
{
    public const string NomeObrigatorio = "O nome do lote é obrigatório.";
    public const string NomeCurto = "O nome do lote precisa ter pelo menos {1} caracteres.";
    public const string NomeMuitoGrande = "O nome do lote pode ter no máximo {1} caracteres.";
    public const string DataNascimentoObrigatoria = "A data de nascimento do lote é obrigatória.";

    public static readonly ApplicationError ErroFormatoDataNascimento =
        new(
            "DATA_NASCIMENTO_INVALIDA",
            "A data de nascimento deve estar no formato yyyy-MM-dd.",
            StatusCodes.Status400BadRequest);

    public static readonly ApplicationError ErroLoteNaoEncontrado =
        new(
            "LOTE_NAO_ENCONTRADO",
            "O lote não foi encontrado.",
            StatusCodes.Status404NotFound);

    public static readonly ApplicationError ErroNomeLoteEmUso =
        new(
            "NOME_LOTE_EM_USO",
            "Este nome de lote já está em uso.",
            StatusCodes.Status409Conflict);
}
