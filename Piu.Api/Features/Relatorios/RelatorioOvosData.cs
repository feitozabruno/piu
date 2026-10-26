namespace Piu.Api.Features.Relatorios;

public sealed record RelatorioOvosData(
    DateTime HoraRecebimento,
    string Lote,
    int NinhoIncubavel,
    int CamaIncubavel,
    int TotalRecebido,
    decimal TemperaturaRecebimento,
    decimal TemperaturaBau,
    string PlacaCaminhao);
