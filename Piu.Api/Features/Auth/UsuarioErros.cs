using Piu.Api.Domain;

namespace Piu.Api.Features.Auth;

public static class UsuarioErros
{
    public static readonly DomainError UsuarioObrigatorio =
        new("USUARIO_OBRIGATORIO", "O nome de usuário é obrigatório.");

    public static readonly DomainError NomeObrigatorio =
        new("NOME_OBRIGATORIO", "O nome é obrigatório.");

    public static readonly DomainError CargoObrigatorio =
        new("CARGO_OBRIGATORIO", "O cargo é obrigatório.");

    public static readonly DomainError IncubatorioObrigatorio =
        new("INCUBATORIO_OBRIGATORIO", "O incubatório é obrigatório.");
}
