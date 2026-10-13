using Piu.Api.Domain;
using Piu.Api.Features.Auth;

namespace Piu.Api.UnitTests.Domain;

public class UsuarioTests
{
    [Fact]
    public void Criar_ComDadosValidos_DeveCriarUsuario()
    {
        const string usuario = "joaosilva";
        const string nome = "João Silva";
        const string cargo = "Operador";
        const string incubatorio = "180";

        var resultado = Usuario.Criar(usuario, nome, cargo, incubatorio);

        Assert.NotNull(resultado);
        Assert.Equal(usuario, resultado.UserName);
        Assert.Equal(nome, resultado.Nome);
        Assert.Equal(cargo, resultado.Cargo);
        Assert.Equal(incubatorio, resultado.Incubatorio);
    }

    [Fact]
    public void Criar_SemUsuario_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "",
                nome: "Maria Silva",
                cargo: "Operador",
                incubatorio: "DRD-180"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("USUARIO_OBRIGATORIO", excecao.Error.Code);
        Assert.Equal("O nome de usuário é obrigatório.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_UsuarioCurto_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "ma",
                nome: "Maria Silva",
                cargo: "Operador",
                incubatorio: "DRD-180"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("USUARIO_CURTO", excecao.Error.Code);
        Assert.Equal("O nome de usuário é muito curto.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_UsuarioMuitoGrande_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "mariasilvamariasilvamariasilvamariasilvamariasilvamariasilva",
                nome: "Maria Silva",
                cargo: "Operador",
                incubatorio: "DRD-180"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("USUARIO_MUITO_GRANDE", excecao.Error.Code);
        Assert.Equal("O nome de usuário é muito grande.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_SemNome_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "mariasilva",
                nome: "",
                cargo: "Operador",
                incubatorio: "DRD-180"
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
            () => Usuario.Criar(
                usuario: "mariasilva",
                nome: "ma",
                cargo: "Operador",
                incubatorio: "DRD-180"
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
            () => Usuario.Criar(
                usuario: "mariasilva",
                nome: "Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva Maria Silva",
                cargo: "Operador",
                incubatorio: "DRD-180"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("NOME_MUITO_GRANDE", excecao.Error.Code);
        Assert.Equal("O nome é muito grande.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_SemCargo_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "mariasilva",
                nome: "Maria Silva",
                cargo: "",
                incubatorio: "DRD-180"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("CARGO_OBRIGATORIO", excecao.Error.Code);
        Assert.Equal("O cargo é obrigatório.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_CargoCurto_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "mariasilva",
                nome: "Maria Silva",
                cargo: "O",
                incubatorio: "DRD-180"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("CARGO_CURTO", excecao.Error.Code);
        Assert.Equal("O cargo é muito curto.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_CargoMuitoGrande_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "mariasilva",
                nome: "Maria Silva",
                cargo: "OperadorOperadorOperadorOperadorOperadorOperadorOperador",
                incubatorio: "DRD-180"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("CARGO_MUITO_GRANDE", excecao.Error.Code);
        Assert.Equal("O cargo é muito grande.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_SemIncubatorio_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "mariasilva",
                nome: "Maria Silva",
                cargo: "Operador",
                incubatorio: ""
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("INCUBATORIO_OBRIGATORIO", excecao.Error.Code);
        Assert.Equal("O incubatório é obrigatório.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_IncubatorioCurto_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "mariasilva",
                nome: "Maria Silva",
                cargo: "Operador",
                incubatorio: "1"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("INCUBATORIO_CURTO", excecao.Error.Code);
        Assert.Equal("O incubatório é muito curto.", excecao.Error.Message);
    }

    [Fact]
    public void Criar_IncubatorioMuitoGrande_DeveLancarExcecao()
    {
        var excecao = Assert.Throws<DomainException>(
            () => Usuario.Criar(
                usuario: "mariasilva",
                nome: "Maria Silva",
                cargo: "Operador",
                incubatorio: "123456789123456789123456789123456789123456789123456789"
            )
        );

        Assert.NotNull(excecao);
        Assert.Equal("INCUBATORIO_MUITO_GRANDE", excecao.Error.Code);
        Assert.Equal("O incubatório é muito grande.", excecao.Error.Message);
    }
}
