using Piu.Api.Exceptions;
using Piu.Api.Features.Auth;

namespace Piu.Api.Tests.Unit.Domain;

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
        Assert.Equal(UsuarioErros.UsuarioObrigatorio.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.UsuarioObrigatorio.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.UsuarioCurto.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.UsuarioCurto.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.UsuarioMuitoGrande.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.UsuarioMuitoGrande.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.NomeObrigatorio.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.NomeObrigatorio.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.NomeCurto.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.NomeCurto.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.NomeMuitoGrande.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.NomeMuitoGrande.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.CargoObrigatorio.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.CargoObrigatorio.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.CargoCurto.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.CargoCurto.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.CargoMuitoGrande.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.CargoMuitoGrande.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.IncubatorioObrigatorio.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.IncubatorioObrigatorio.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.IncubatorioCurto.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.IncubatorioCurto.Message, excecao.Error.Message);
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
        Assert.Equal(UsuarioErros.IncubatorioMuitoGrande.Code, excecao.Error.Code);
        Assert.Equal(UsuarioErros.IncubatorioMuitoGrande.Message, excecao.Error.Message);
    }
}
