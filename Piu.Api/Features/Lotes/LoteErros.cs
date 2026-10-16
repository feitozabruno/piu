using Piu.Api.Domain;

namespace Piu.Api.Features.Lotes;

public static class LoteErros
{
    public static readonly DomainError NomeObrigatorio =
        new("NOME_LOTE_OBRIGATORIO", "O nome do lote é obrigatório.");

    public static readonly DomainError NomeCurto =
        new("NOME_LOTE_CURTO", "O nome do lote é muito curto.");

    public static readonly DomainError NomeMuitoGrande =
        new("NOME_LOTE_MUITO_GRANDE", "O nome do lote é muito grande.");

    public static readonly DomainError DataNascimentoObrigatoria =
        new("DATA_NASCIMENTO_OBRIGATORIA", "A data de nascimento é obrigatória.");

    public static readonly DomainError DataNascimentoInvalida =
        new("DATA_NASCIMENTO_INVALIDA", "A data de nascimento é inválida.");

    public static readonly DomainError DataReferenciaInvalida =
        new("DATA_REFERENCIA_INVALIDA", "A data de referência é inválida.");

    public static readonly DomainError IncubatorioObrigatorio =
        new("INCUBATORIO_LOTE_OBRIGATORIO", "O incubatório do lote é obrigatório.");
}
