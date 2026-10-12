using Piu.Api.Domain;

namespace Piu.Api.Features.Incubatorios;

public static class IncubatorioErros
{
    public static readonly DomainError IdentificadorObrigatorio =
        new("IDENTIFICADOR_OBRIGATORIO", "O identificador é obrigatório.");

    public static readonly DomainError IdentificadorCurto =
        new("IDENTIFICADOR_CURTO", "O identificador é muito curto.");

    public static readonly DomainError IdentificadorMuitoGrande =
        new("IDENTIFICADOR_MUITO_GRANDE", "O identificador é muito grande.");

    public static readonly DomainError NomeObrigatorio =
        new("NOME_OBRIGATORIO", "O nome é obrigatório.");

    public static readonly DomainError NomeCurto =
        new("NOME_CURTO", "O nome é muito curto.");

    public static readonly DomainError NomeMuitoGrande =
        new("NOME_MUITO_GRANDE", "O nome é muito grande.");

    public static readonly DomainError SiglaObrigatoria =
        new("SIGLA_OBRIGATORIA", "A sigla é obrigatória.");

    public static readonly DomainError SiglaCurta =
        new("SIGLA_CURTA", "A sigla é muito curta.");

    public static readonly DomainError SiglaMuitoGrande =
        new("SIGLA_MUITO_GRANDE", "A sigla é muito grande.");
}
