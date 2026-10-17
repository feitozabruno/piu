namespace Piu.Api.Features.Ovos.ReceberOvo;

public sealed record ReceberOvoResponse(
    Guid Id,
    string Lote,
    DateOnly DataPostura,
    int NinhoIncubavel,
    int CamaIncubavel,
    int TotalRecebido,
    string Idade,
    decimal TemperaturaRecebimento,
    decimal TemperaturaBau,
    string PlacaCaminhao,
    string Motorista,
    DateTime HoraRecebimento,
    string Observacao,
    DateTime CriadoEm)
{
    public static ReceberOvoResponse From(Ovo ovo) => new(
        ovo.Id,
        ovo.Lote.Nome,
        ovo.DataPostura,
        ovo.NinhoIncubavel,
        ovo.CamaIncubavel,
        ovo.TotalRecebido,
        ovo.Idade,
        ovo.TemperaturaRecebimento,
        ovo.TemperaturaBau,
        ovo.PlacaCaminhao,
        ovo.Motorista,
        ovo.HoraRecebimento,
        ovo.Observacao,
        ovo.CriadoEm);
}
