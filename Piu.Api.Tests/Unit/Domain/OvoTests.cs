using Piu.Api.Exceptions;
using Piu.Api.Features.Lotes;
using Piu.Api.Features.Ovos;

namespace Piu.Api.Tests.Unit.Domain;

public sealed class OvoTests
{
    [Fact]
    public void Receber_ComDadosValidos_DeveCalcularTotalEIdade()
    {
        var lote = CriarLote();
        var dados = CriarDados();

        var resultado = Ovo.Receber(lote, dados);

        Assert.Equal(33, resultado.TotalRecebido);
        Assert.Equal("30,6", resultado.Idade);
        Assert.Equal("ABC1234", resultado.PlacaCaminhao);
        Assert.Equal("Maria", resultado.Motorista);
    }

    [Fact]
    public void Receber_ComTotalQueExcedeInt_DeveLancarExcecao()
    {
        var dados = CriarDados() with
        {
            NinhoIncubavel = int.MaxValue,
            CamaIncubavel = 1
        };

        var excecao = Assert.Throws<DomainException>(
            () => Ovo.Receber(CriarLote(), dados));

        Assert.Equal(OvoErros.TotalOvosExcedeLimite.Code, excecao.Error.Code);
    }

    [Fact]
    public void Receber_ComTemperaturaComMaisDeDuasCasas_DeveLancarExcecao()
    {
        var dados = CriarDados() with { TemperaturaBau = 18.123m };

        var excecao = Assert.Throws<DomainException>(
            () => Ovo.Receber(CriarLote(), dados));

        Assert.Equal(OvoErros.TemperaturasInvalidas.Code, excecao.Error.Code);
    }

    [Fact]
    public void Receber_ComDataAnteriorAoNascimento_DeveLancarExcecao()
    {
        var dados = CriarDados() with { DataPostura = new DateOnly(2026, 3, 13) };

        var excecao = Assert.Throws<DomainException>(
            () => Ovo.Receber(CriarLote(), dados));

        Assert.Equal(OvoErros.DataPosturaInvalida.Code, excecao.Error.Code);
    }

    private static Lote CriarLote() =>
        Lote.Criar("DRD01-010", new DateOnly(2026, 3, 14), "INC-001");

    private static OvoRecebimentoDados CriarDados() => new()
    {
        ClientId = Guid.NewGuid(),
        DataPostura = new DateOnly(2026, 10, 16),
        NinhoIncubavel = 30,
        CamaIncubavel = 3,
        TemperaturaRecebimento = 18.00m,
        TemperaturaBau = 18.50m,
        PlacaCaminhao = " ABC1234 ",
        Motorista = " Maria ",
        HoraRecebimento = new DateTime(2026, 10, 16, 9, 30, 0, DateTimeKind.Utc),
        Observacao = " Observação "
    };
}
