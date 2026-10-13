using Piu.Api.Application;

namespace Piu.Api.Features.Auth.CadastrarUsuario;

public static class CadastrarUsuarioErros
{
    public const string NomeUsuarioObrigatorio = "O nome de usuário é obrigatório.";
    public const string NomeUsuarioCurto = "O nome de usuário precisa ter pelo menos {1} caracteres.";
    public const string NomeUsuarioMuitoGrande = "O nome de usuário pode ter no máximo {1} caracteres.";
    public const string NomeUsuarioEmUso = "Este nome de usuário já está em uso.";
    public const string SenhaObrigatoria = "A senha é obrigatória.";
    public const string SenhaFraca = "A senha precisa ter pelo menos {1} caracteres.";
    public const string SenhaMuitoGrande = "A senha pode ter no máximo {1} caracteres.";
    public const string NomeObrigatorio = "O nome é obrigatório.";
    public const string NomeCurto = "O nome precisa ter pelo menos {1} caracteres.";
    public const string NomeMuitoGrande = "O nome pode ter no máximo {1} caracteres.";
    public const string CargoObrigatorio = "O cargo é obrigatório.";
    public const string CargoCurto = "O cargo precisa ter pelo menos {1} caracteres.";
    public const string CargoMuitoGrande = "O cargo pode ter no máximo {1} caracteres.";
    public const string NomeIncubatorioObrigatorio = "O nome do incubatório é obrigatório.";
    public const string NomeIncubatorioCurto = "O nome do incubatório precisa ter pelo menos {1} caracteres.";
    public const string NomeIncubatorioMuitoGrande = "O nome do incubatorio pode ter no máximo {1} caracteres.";
    public static readonly ApplicationError ErroUsuarioEmUso = new("USUARIO_EM_USO", NomeUsuarioEmUso, StatusCodes.Status409Conflict);
    public static readonly ApplicationError ErroSenhaInvalida = new("SENHA_INVALIDA", "A senha não atende aos requisitos de segurança.");
    public static readonly ApplicationError ErroIncubatorioInvalido =
    new("INCUBATORIO_INVALIDO", "O incubatório informado não existe.", StatusCodes.Status422UnprocessableEntity);
    public static readonly ApplicationError FalhaAoCadastrar = new("FALHA_AO_CADASTRAR_USUARIO", "Não foi possível cadastrar o usuário.");
}
