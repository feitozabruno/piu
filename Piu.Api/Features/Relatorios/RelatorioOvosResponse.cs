namespace Piu.Api.Features.Relatorios;

public sealed record RelatorioOvosResponse(
    DateTime HoraRecebimento,
    string Lote,
    int NinhoIncubavel,
    int CamaIncubavel,
    int TotalRecebido,
    decimal TemperaturaRecebimento,
    decimal TemperaturaBau,
    string PlacaCaminhao
);
