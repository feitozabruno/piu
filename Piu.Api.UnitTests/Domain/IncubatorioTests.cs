using Piu.Api.Domain;
using Piu.Api.Features.Incubatorios;

namespace Piu.Api.UnitTests.Domain;

public class IncubatorioTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarIncubatorio()
    {
        const string identificador = "180";
        const string nome = "Incubatório Dourados";
        const string sigla = "DRD";

        var resultado = Incubatorio.Criar(identificador, nome, sigla);

        Assert.NotNull(resultado);
        Assert.Equal(identificador, resultado.Identificador);
        Assert.Equal(nome, resultado.Nome);
        Assert.Equal(sigla, resultado.Sigla);
    }

    [Fact]
    public void Criar_SemIdentificador_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Incubatorio.Criar(
                identificador: "",
                nome: "Incubatório Dourados",
                sigla: "DRD"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("IDENTIFICADOR_OBRIGATORIO", excecao.Error.Code);
        Assert.Equal("O identificador é obrigatório.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_IdentificadorCurto_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Incubatorio.Criar(
                identificador: "1",
                nome: "Incubatório Dourados",
                sigla: "DRD"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("IDENTIFICADOR_CURTO", excecao.Error.Code);
        Assert.Equal("O identificador é muito curto.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_IdentificadorMuitoGrande_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Incubatorio.Criar(
                identificador: "123456789012345678901234567890123456789012345678901234567890",
                nome: "Incubatório Dourados",
                sigla: "DRD"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("IDENTIFICADOR_MUITO_GRANDE", excecao.Error.Code);
        Assert.Equal("O identificador é muito grande.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_SemNome_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Incubatorio.Criar(
                identificador: "180",
                nome: "",
                sigla: "DRD"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("NOME_OBRIGATORIO", excecao.Error.Code);
        Assert.Equal("O nome é obrigatório.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_NomeCurto_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Incubatorio.Criar(
                identificador: "180",
                nome: "In",
                sigla: "DRD"
            )
        );
        Assert.NotNull(excecao);
        Assert.Equal("NOME_CURTO", excecao.Error.Code);
        Assert.Equal("O nome é muito curto.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_NomeMuitoGrande_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Incubatorio.Criar(
                identificador: "180",
                nome: "IncubatorioIncubatorioIncubatorioIncubatorioIncubatorioIncubatorioIncubatorioIncubatorioIncubatorioIncubatorioIncubatorioIncubatorioIncubatorioIncubatorio",
                sigla: "DRD"
            )
        );
        Assert.NotNull(excecao);
        Assert.Equal("NOME_MUITO_GRANDE", excecao.Error.Code);
        Assert.Equal("O nome é muito grande.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_SemSigla_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Incubatorio.Criar(
                identificador: "180",
                nome: "Incubatório Dourados",
                sigla: ""
            )
        );
        Assert.NotNull(excecao);
        Assert.Equal("SIGLA_OBRIGATORIA", excecao.Error.Code);
        Assert.Equal("A sigla é obrigatória.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_SiglaCurta_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Incubatorio.Criar(
                identificador: "180",
                nome: "Incubatório Dourados",
                sigla: "D"
            )
        );
        Assert.NotNull(excecao);
        Assert.Equal("SIGLA_CURTA", excecao.Error.Code);
        Assert.Equal("A sigla é muito curta.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_SiglaMuitoGrande_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Incubatorio.Criar(
                identificador: "180",
                nome: "Incubatório Dourados",
                sigla: "DOURADOSDOURADOSDOURADOSDOURADOS"
            )
        );
        Assert.NotNull(excecao);
        Assert.Equal("SIGLA_MUITO_GRANDE", excecao.Error.Code);
        Assert.Equal("A sigla é muito grande.", excecao.Error.Message);
    }
}
