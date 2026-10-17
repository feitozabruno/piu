using Piu.Api.Application;

namespace Piu.Api.Features.Auth;

public static class AuthErros
{
    public static readonly ApplicationError UsuarioNaoAutenticado =
        new("USUARIO_NAO_AUTENTICADO", "O usuário autenticado não foi encontrado.", StatusCodes.Status401Unauthorized);
}
