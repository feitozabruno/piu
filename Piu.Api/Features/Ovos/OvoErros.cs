using Piu.Api.Exceptions;

namespace Piu.Api.Features.Ovos;

public static class OvoErros
{
    public static readonly DomainError LoteObrigatorio =
        new("LOTE_OVO_OBRIGATORIO", "O lote do ovo é obrigatório.");

    public static readonly DomainError DadosObrigatorios =
        new("DADOS_OVO_OBRIGATORIOS", "Os dados de recebimento do ovo são obrigatórios.");

    public static readonly DomainError DataPosturaObrigatoria =
        new("DATA_POSTURA_OVO_OBRIGATORIA", "A data de postura é obrigatória.");

    public static readonly DomainError DataPosturaInvalida =
        new("DATA_POSTURA_OVO_INVALIDA", "A data de postura é inválida.");

    public static readonly DomainError NinhoIncubavelInvalidos =
        new("OVOS_NINHO_INVALIDOS", "A quantidade de ovos de ninho deve ser maior ou igual a zero.");

    public static readonly DomainError CamaIncubavelInvalidos =
        new("OVOS_CAMA_INVALIDOS", "A quantidade de ovos de cama deve ser maior ou igual a zero.");

    public static readonly DomainError TotalOvosInvalido =
        new("TOTAL_OVOS_INVALIDO", "A quantidade total de ovos deve ser maior que zero.");

    public static readonly DomainError TotalOvosExcedeLimite =
        new("TOTAL_OVOS_MUITO_GRANDE", "A quantidade total de ovos excede o limite permitido.");

    public static readonly DomainError TemperaturasInvalidas =
        new("TEMPERATURAS_INVALIDAS", "As temperaturas devem ter no máximo duas casas decimais e estar dentro do limite permitido.");

    public static readonly DomainError PlacaCaminhaoObrigatoria =
        new("PLACA_CAMINHAO_OVO_OBRIGATORIA", "A placa do caminhão é obrigatória.");

    public static readonly DomainError MotoristaObrigatorio =
        new("MOTORISTA_OVO_OBRIGATORIO", "O motorista é obrigatório.");

    public static readonly DomainError HoraRecebimentoObrigatoria =
        new("HORA_RECEBIMENTO_OVO_OBRIGATORIA", "A hora de recebimento é obrigatória.");
}
