using Piu.Api.Exceptions;

namespace Piu.Api.Features.Admin;

public static class AdminErrors
{
    public static readonly ApplicationError UsuarioEmUso = new(
        "USUARIO_EM_USO", "O nome de usuário informado já está em uso.", StatusCodes.Status409Conflict);

    public static readonly ApplicationError IncubatorioEmUso = new(
        "INCUBATORIO_EM_USO", "O identificador do incubatório informado já está em uso.", StatusCodes.Status409Conflict);

    public static readonly ApplicationError FalhaAoCriarUsuario = new(
        "FALHA_AO_CRIAR_USUARIO", "Não foi possível criar o usuário.", StatusCodes.Status400BadRequest);

    public static readonly ApplicationError IncubatorioNaoEncontrado = new(
        "INCUBATORIO_NAO_ENCONTRADO", "O incubatório informado não existe.", StatusCodes.Status404NotFound);
}
