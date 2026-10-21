using System.ComponentModel.DataAnnotations;

namespace Piu.Api.Features.Ovos.ReceberOvo;

public sealed record ReceberOvoRequest(
    Guid ClientId,

    [Required(ErrorMessage = ReceberOvoErros.LoteObrigatorio)]
    string Lote,

    [Required(ErrorMessage = ReceberOvoErros.DataPosturaObrigatoria)]
    string DataPostura,

    [Range(0, 500000, ErrorMessage = ReceberOvoErros.NinhoIncubavelInvalido)]
    int NinhoIncubavel,

    [Range(0, 500000, ErrorMessage = ReceberOvoErros.CamaIncubavelInvalido)]
    int CamaIncubavel,

    decimal TemperaturaOvo,
    decimal TemperaturaBau,

    [Required(ErrorMessage = ReceberOvoErros.PlacaCaminhaoObrigatoria)]
    string PlacaCaminhao,

    [Required(ErrorMessage = ReceberOvoErros.MotoristaObrigatorio)]
    string Motorista,

    [Required(ErrorMessage = ReceberOvoErros.HoraRecebimentoObrigatoria)]
    string HoraRecebimento,

    string? Observacao
);
