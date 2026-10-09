using Piu.Api.Application;

namespace Piu.Api.Features.Auth.FazerLogin;

public static class FazerLoginErros
{
    public const string UsuarioObrigatorio = "Informe o usuário.";
    public const string SenhaObrigatoria = "Informe a senha.";
    public static readonly ApplicationError CredenciaisInvalidas = new("CREDENCIAIS_INVALIDAS", "Usuário ou senha incorretos.", StatusCodes.Status401Unauthorized);
    public static readonly ApplicationError ContaBloqueada = new("CONTA_BLOQUEADA", "Muitas tentativas de login. Aguarde alguns minutos e tente novamente.", StatusCodes.Status429TooManyRequests);
}
