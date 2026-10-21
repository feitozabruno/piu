namespace Piu.Api.Features.Ovos;

public sealed record OvoRecebimentoDados
{
    public required Guid ClientId { get; init; }
    public required DateOnly DataPostura { get; init; }
    public required int NinhoIncubavel { get; init; }
    public required int CamaIncubavel { get; init; }
    public required decimal TemperaturaRecebimento { get; init; }
    public required decimal TemperaturaBau { get; init; }
    public required string PlacaCaminhao { get; init; }
    public required string Motorista { get; init; }
    public required DateTime HoraRecebimento { get; init; }
    public string? Observacao { get; init; }
}
