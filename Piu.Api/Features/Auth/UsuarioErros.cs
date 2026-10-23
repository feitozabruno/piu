using Piu.Api.Exceptions;

namespace Piu.Api.Features.Auth;

public static class UsuarioErros
{
    public static readonly DomainError UsuarioObrigatorio =
        new("USUARIO_OBRIGATORIO", "O nome de usuário é obrigatório.");

    public static readonly DomainError UsuarioCurto =
        new("USUARIO_CURTO", "O nome de usuário é muito curto.");

    public static readonly DomainError UsuarioMuitoGrande =
        new("USUARIO_MUITO_GRANDE", "O nome de usuário é muito grande.");

    public static readonly DomainError NomeObrigatorio =
        new("NOME_OBRIGATORIO", "O nome é obrigatório.");

    public static readonly DomainError NomeCurto =
        new("NOME_CURTO", "O nome é muito curto.");

    public static readonly DomainError NomeMuitoGrande =
        new("NOME_MUITO_GRANDE", "O nome é muito grande.");

    public static readonly DomainError CargoObrigatorio =
        new("CARGO_OBRIGATORIO", "O cargo é obrigatório.");

    public static readonly DomainError CargoCurto =
        new("CARGO_CURTO", "O cargo é muito curto.");

    public static readonly DomainError CargoMuitoGrande =
        new("CARGO_MUITO_GRANDE", "O cargo é muito grande.");

    public static readonly DomainError IncubatorioObrigatorio =
        new("INCUBATORIO_OBRIGATORIO", "O incubatório é obrigatório.");

    public static readonly DomainError IncubatorioCurto =
        new("INCUBATORIO_CURTO", "O incubatório é muito curto.");

    public static readonly DomainError IncubatorioMuitoGrande =
        new("INCUBATORIO_MUITO_GRANDE", "O incubatório é muito grande.");
}
