using Piu.Api.Features.Lotes;
using Piu.Api.Domain;

namespace Piu.Api.Features.Ovos;

public sealed class Ovo
{
    public Guid Id { get; private set; }
    public Lote Lote { get; private set; } = null!;
    public Guid LoteId { get; private set; }
    public DateOnly DataPostura { get; private set; }
    public int NinhoIncubavel { get; private set; }
    public int CamaIncubavel { get; private set; }
    public string Idade { get; private set; } = string.Empty;
    public int TotalRecebido { get; private set; }
    public DateTime CriadoEm { get; private set; } = TruncarParaMicrossegundos(DateTime.UtcNow);

    public decimal TemperaturaRecebimento { get; private set; }
    public decimal TemperaturaBau { get; private set; }
    public string PlacaCaminhao { get; private set; } = string.Empty;
    public string Motorista { get; private set; } = string.Empty;
    public DateTime HoraRecebimento { get; private set; }
    public string Observacao { get; private set; } = string.Empty;

    private Ovo() { }

    private Ovo(Lote lote, OvoRecebimentoDados dados)
    {
        Id = Guid.NewGuid();

        Lote = lote;
        LoteId = lote.Id;
        DataPostura = dados.DataPostura;
        NinhoIncubavel = dados.NinhoIncubavel;
        CamaIncubavel = dados.CamaIncubavel;
        TotalRecebido = dados.NinhoIncubavel + dados.CamaIncubavel;
        Idade = lote.CalcularIdadeEm(dados.DataPostura);
        TemperaturaRecebimento = dados.TemperaturaRecebimento;
        TemperaturaBau = dados.TemperaturaBau;
        PlacaCaminhao = dados.PlacaCaminhao;
        Motorista = dados.Motorista;
        HoraRecebimento = TruncarParaMicrossegundos(dados.HoraRecebimento);
        Observacao = dados.Observacao ?? string.Empty;
    }

    public static Ovo Receber(Lote? lote, OvoRecebimentoDados? dados)
    {
        if (lote is null) throw new DomainException(OvoErros.LoteObrigatorio);
        if (dados is null) throw new DomainException(OvoErros.DadosObrigatorios);

        if (dados.DataPostura == default) throw new DomainException(OvoErros.DataPosturaObrigatoria);
        if (dados.DataPostura < lote.DataNascimento)
            throw new DomainException(OvoErros.DataPosturaInvalida);

        if (dados.NinhoIncubavel < 0) throw new DomainException(OvoErros.NinhoIncubavelInvalidos);
        if (dados.CamaIncubavel < 0) throw new DomainException(OvoErros.CamaIncubavelInvalidos);
        if (dados.NinhoIncubavel == 0 && dados.CamaIncubavel == 0)
            throw new DomainException(OvoErros.TotalOvosInvalido);
        if ((long)dados.NinhoIncubavel + dados.CamaIncubavel > int.MaxValue)
            throw new DomainException(OvoErros.TotalOvosExcedeLimite);

        if (!TemperaturaValida(dados.TemperaturaRecebimento)
            || !TemperaturaValida(dados.TemperaturaBau))
        {
            throw new DomainException(OvoErros.TemperaturasInvalidas);
        }

        if (string.IsNullOrWhiteSpace(dados.PlacaCaminhao))
            throw new DomainException(OvoErros.PlacaCaminhaoObrigatoria);

        if (string.IsNullOrWhiteSpace(dados.Motorista))
            throw new DomainException(OvoErros.MotoristaObrigatorio);

        if (dados.HoraRecebimento == default)
            throw new DomainException(OvoErros.HoraRecebimentoObrigatoria);

        dados = dados with
        {
            PlacaCaminhao = dados.PlacaCaminhao.Trim(),
            Motorista = dados.Motorista.Trim(),
            Observacao = dados.Observacao?.Trim() ?? string.Empty
        };

        return new Ovo(lote, dados);
    }

    private static bool TemperaturaValida(decimal temperatura) =>
        temperatura is >= -99999999.99m and <= 99999999.99m
        && decimal.Round(temperatura, 2) == temperatura;

    private static DateTime TruncarParaMicrossegundos(DateTime valor) =>
        new(valor.Ticks - valor.Ticks % 10, valor.Kind);
}
