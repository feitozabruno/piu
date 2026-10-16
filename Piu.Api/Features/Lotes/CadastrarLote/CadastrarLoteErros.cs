using Piu.Api.Application;

namespace Piu.Api.Features.Lotes.CadastrarLote;

public static class CadastrarLoteErros
{
    public const string NomeObrigatorio = "O nome do lote é obrigatório.";
    public const string NomeCurto = "O nome do lote precisa ter pelo menos {1} caracteres.";
    public const string NomeMuitoGrande = "O nome do lote pode ter no máximo {1} caracteres.";

    public static readonly ApplicationError ErroNomeLoteEmUso =
        new("NOME_LOTE_EM_USO", "Este nome de lote já está em uso.", StatusCodes.Status409Conflict);

    public static readonly ApplicationError UsuarioNaoAutenticado =
        new("USUARIO_NAO_AUTENTICADO", "O usuário autenticado não foi encontrado.", StatusCodes.Status401Unauthorized);

    public const string DataNascimentoObrigatoria = "A data de nascimento do lote é obrigatória.";
    public const string DataNascimentoInvalida = "A data de nascimento é inválida.";

    public static readonly ApplicationError ErroFormatoDataNascimento =
        new(
            "DATA_NASCIMENTO_INVALIDA",
            "A data de nascimento deve estar no formato yyyy-MM-dd.",
            StatusCodes.Status400BadRequest);
}
