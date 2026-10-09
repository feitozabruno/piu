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
        const string incubatorio = "DRD-180";

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
}
