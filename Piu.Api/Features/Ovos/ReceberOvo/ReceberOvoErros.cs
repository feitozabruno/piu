using Piu.Api.Application;

namespace Piu.Api.Features.Ovos.ReceberOvo;

public static class ReceberOvoErros
{
    public const string LoteObrigatorio = "O lote é obrigatório.";
    public const string DataPosturaObrigatoria = "A data de postura é obrigatória.";
    public const string NinhoIncubavelInvalido = "A quantidade de ovos de ninho deve ser maior ou igual a zero.";
    public const string CamaIncubavelInvalido = "A quantidade de ovos de cama deve ser maior ou igual a zero.";
    public const string PlacaCaminhaoObrigatoria = "A placa do caminhão é obrigatória.";
    public const string MotoristaObrigatorio = "O motorista é obrigatório.";
    public const string HoraRecebimentoObrigatoria = "A hora de recebimento é obrigatória.";

    public static readonly ApplicationError ErroDataPosturaFutura =
        new("DATA_POSTURA_FUTURA", "A data de postura não pode ser futura.", StatusCodes.Status400BadRequest);

    public static readonly ApplicationError ErroFormatoDataPostura =
        new("DATA_POSTURA_INVALIDA", "A data de postura deve estar no formato yyyy-MM-dd.", StatusCodes.Status400BadRequest);

    public static readonly ApplicationError ErroFormatoHoraRecebimento =
        new("HORA_RECEBIMENTO_INVALIDA", "A hora de recebimento deve estar em um formato de data e hora válido.", StatusCodes.Status400BadRequest);

    public static readonly ApplicationError ErroHoraRecebimentoFutura =
        new("HORA_RECEBIMENTO_FUTURA", "A hora de recebimento não pode ser futura.", StatusCodes.Status400BadRequest);

    public static readonly ApplicationError ErroLoteNaoEncontrado =
        new("LOTE_NAO_ENCONTRADO", "O lote informado não foi encontrado.", StatusCodes.Status404NotFound);

    public static readonly ApplicationError ErroTotalOvosInvalido =
        new("TOTAL_OVOS_INVALIDO", "A quantidade total de ovos deve ser maior que zero.", StatusCodes.Status400BadRequest);
}
