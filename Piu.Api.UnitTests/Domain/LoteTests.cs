using Piu.Api.Domain;
using Piu.Api.Features.Lotes;

namespace Piu.Api.UnitTests.Domain;

public class LoteTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarLote()
    {
        const string nome = "DRD01-010";
        var dataNascimento = new DateOnly(2026, 3, 14);

        var resultado = Lote.Criar(nome, dataNascimento, "INC-001");

        Assert.NotNull(resultado);
        Assert.NotEqual(Guid.Empty, resultado.Id);
        Assert.Equal(nome.ToUpperInvariant(), resultado.Nome);
        Assert.Equal(dataNascimento, resultado.DataNascimento);
    }

    [Fact]
    public void Criar_SemNome_DeveLancarExcecao()
    {
        var dataNascimento = new DateOnly(2026, 3, 14);

        var excecao = Assert.Throws<DomainException>(
            () => Lote.Criar(
                nome: "",
                dataNascimento: dataNascimento,
                incubatorioIdentificador: "INC-001"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal(LoteErros.NomeObrigatorio.Code, excecao.Error.Code);
        Assert.Equal(LoteErros.NomeObrigatorio.Message, excecao.Error.Message);
    }

    [Fact]
    public void Criar_ComNomeCurto_DeveLancarExcecao()
    {
        const string nome = "DR";
        var dataNascimento = new DateOnly(2026, 3, 14);

        var excecao = Assert.Throws<DomainException>(
            () => Lote.Criar(
                nome: nome,
                dataNascimento: dataNascimento,
                incubatorioIdentificador: "INC-001"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal(LoteErros.NomeCurto.Code, excecao.Error.Code);
        Assert.Equal(LoteErros.NomeCurto.Message, excecao.Error.Message);
    }

    [Fact]
    public void Criar_ComNomeMuitoGrande_DeveLancarExcecao()
    {
        const string nome = "DRD01-010-DRD01-010-DRD01-010-DRD01-010-DRD01-010";
        var dataNascimento = new DateOnly(2026, 3, 14);

        var excecao = Assert.Throws<DomainException>(
            () => Lote.Criar(
                nome: nome,
                dataNascimento: dataNascimento,
                incubatorioIdentificador: "INC-001"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal(LoteErros.NomeMuitoGrande.Code, excecao.Error.Code);
        Assert.Equal(LoteErros.NomeMuitoGrande.Message, excecao.Error.Message);
    }

    [Fact]
    public void Criar_SemDataNascimento_DeveLancarExcecao()
    {
        const string nome = "DRD01-010";

        var excecao = Assert.Throws<DomainException>(
            () => Lote.Criar(
                nome: nome,
                dataNascimento: default,
                incubatorioIdentificador: "INC-001"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal(LoteErros.DataNascimentoObrigatoria.Code, excecao.Error.Code);
        Assert.Equal(LoteErros.DataNascimentoObrigatoria.Message, excecao.Error.Message);
    }

    [Fact]
    public void Criar_ComDataNascimentoFutura_DeveLancarExcecao()
    {
        const string nome = "DRD01-010";
        var dataNascimento = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var excecao = Assert.Throws<DomainException>(
            () => Lote.Criar(
                nome: nome,
                dataNascimento: dataNascimento,
                incubatorioIdentificador: "INC-001"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal(LoteErros.DataNascimentoInvalida.Code, excecao.Error.Code);
        Assert.Equal(LoteErros.DataNascimentoInvalida.Message, excecao.Error.Message);
    }

    [Fact]
    public void CalcularIdadeEm_ComDataReferenciaValida_DeveRetornarIdade()
    {
        const string nome = "DRD01-010";
        var dataNascimento = new DateOnly(2026, 3, 14);
        var lote = Lote.Criar(nome, dataNascimento, "INC-001");

        var dataReferencia = new DateOnly(2026, 10, 16);

        var resultado = lote.CalcularIdadeEm(dataReferencia);

        Assert.Equal("30,6", resultado);
    }

    [Fact]
    public void CalcularIdadeEm_ComDataReferenciaAnteriorADataNascimento_DeveLancarExcecao()
    {
        const string nome = "DRD01-010";
        var dataNascimento = new DateOnly(2026, 3, 14);
        var lote = Lote.Criar(nome, dataNascimento, "INC-001");

        var dataReferencia = new DateOnly(2025, 3, 14);

        var excecao = Assert.Throws<DomainException>(
            () => lote.CalcularIdadeEm(dataReferencia)
        );

        Assert.NotNull(excecao);
        Assert.Equal(LoteErros.DataReferenciaInvalida.Code, excecao.Error.Code);
        Assert.Equal(LoteErros.DataReferenciaInvalida.Message, excecao.Error.Message);
    }
}
