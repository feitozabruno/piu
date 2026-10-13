using System.Net;
using System.Net.Http.Json;
using Piu.Api.Features.Auth;
using Piu.Api.Features.Auth.CadastrarUsuario;
using Piu.Api.IntegrationTests.Fixtures;

namespace Piu.Api.IntegrationTests.Features.Auth;

[Collection("API")]
public sealed class CadastrarUsuarioEndpointTests(CustomWebApplicationFactory factory) : ApiIntegrationTestBase(factory)
{
    [Fact]
    public async Task Cadastrar_ComDadosValidos_DeveRetornarCreated()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "maria",
            Senha: "senha123",
            Nome: "Maria Silva",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var usuario = await response.Content
            .ReadFromJsonAsync<CadastrarUsuarioResponse>();

        Assert.NotNull(usuario);
        Assert.Equal("maria", usuario.Usuario);
        Assert.Equal("Maria Silva", usuario.Nome);
        Assert.Equal("INC-001", usuario.Incubatorio);
    }

    [Fact]
    public async Task Cadastrar_ComIncubatorioInvalido_DeveRetornarUnprocessableEntity()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "joao",
            Senha: "senha123",
            Nome: "Joao Silva",
            Cargo: "Operador",
            Incubatorio: "INC-999");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(
            HttpStatusCode.UnprocessableEntity,
            response.StatusCode);

        var erro = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

        Assert.NotNull(erro);

        Assert.Equal(
            CadastrarUsuarioErros.ErroIncubatorioInvalido.Code,
            erro.Code);

        Assert.Equal(
            CadastrarUsuarioErros.ErroIncubatorioInvalido.Message,
            erro.Detail);

        Assert.Equal(
            CadastrarUsuarioErros.ErroIncubatorioInvalido.StatusCode,
            erro.Status);
    }

    [Fact]
    public async Task Cadastrar_ComUsuarioDuplicado_DeveRetornarConflict()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "ana",
            Senha: "senha123",
            Nome: "Ana Silva",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var primeiro = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.Created, primeiro.StatusCode);

        var segundo = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.Conflict, segundo.StatusCode);

        var erro = await segundo.Content.ReadFromJsonAsync<ApiErrorResponse>();

        Assert.NotNull(erro);
        Assert.Equal(CadastrarUsuarioErros.ErroUsuarioEmUso.Code, erro.Code);
        Assert.Equal(CadastrarUsuarioErros.ErroUsuarioEmUso.Message, erro.Detail);
        Assert.Equal(CadastrarUsuarioErros.ErroUsuarioEmUso.StatusCode, erro.Status);
    }

    [Fact]
    public async Task Cadastrar_SemUsuario_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "",
            Senha: "123456",
            Nome: "Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Usuario", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.NomeUsuarioObrigatorio,
            excecao.Errors["Usuario"]);
    }

    [Fact]
    public async Task Cadastrar_ComUsuarioCurto_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "ca",
            Senha: "123456",
            Nome: "Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Usuario", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.NomeUsuarioCurto.Replace("{1}", UsuarioLimites.UsuarioMin.ToString()),
            excecao.Errors["Usuario"]);
    }

    [Fact]
    public async Task Cadastrar_ComUsuarioMuitoGrande_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlosilva1234567890carlosilva1234567890carlosilva1234567890",
            Senha: "123456",
            Nome: "Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Usuario", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.NomeUsuarioMuitoGrande.Replace("{1}", UsuarioLimites.UsuarioMax.ToString()),
            excecao.Errors["Usuario"]);
    }

    [Fact]
    public async Task Cadastrar_SemSenha_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "",
            Nome: "Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Senha", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.SenhaObrigatoria,
            excecao.Errors["Senha"]);
    }

    [Fact]
    public async Task Cadastrar_ComSenhaFraca_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123",
            Nome: "Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Senha", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.SenhaFraca.Replace("{1}", UsuarioLimites.SenhaMin.ToString()),
            excecao.Errors["Senha"]);
    }

    [Fact]
    public async Task Cadastrar_ComSenhaMuitoGrande_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "1234567890123456789012345678901234567890",
            Nome: "Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Senha", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.SenhaMuitoGrande.Replace("{1}", UsuarioLimites.SenhaMax.ToString()),
            excecao.Errors["Senha"]);
    }

    [Fact]
    public async Task Cadastrar_SemNome_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123456",
            Nome: "",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Nome", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.NomeObrigatorio,
            excecao.Errors["Nome"]);
    }

    [Fact]
    public async Task Cadastrar_ComNomeCurto_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123456",
            Nome: "Ca",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Nome", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.NomeCurto.Replace("{1}", UsuarioLimites.NomeMin.ToString()),
            excecao.Errors["Nome"]);
    }

    [Fact]
    public async Task Cadastrar_ComNomeMuitoGrande_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123456",
            Nome: "Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Nome", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.NomeMuitoGrande.Replace("{1}", UsuarioLimites.NomeMax.ToString()),
            excecao.Errors["Nome"]);
    }

    [Fact]
    public async Task Cadastrar_SemCargo_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123456",
            Nome: "Carlos Silva",
            Cargo: "",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Cargo", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.CargoObrigatorio,
            excecao.Errors["Cargo"]);
    }

    [Fact]
    public async Task Cadastrar_ComCargoCurto_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123456",
            Nome: "Carlos Silva",
            Cargo: "O",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Cargo", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.CargoCurto.Replace("{1}", UsuarioLimites.CargoMin.ToString()),
            excecao.Errors["Cargo"]);
    }

    [Fact]
    public async Task Cadastrar_ComCargoMuitoGrande_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123456",
            Nome: "Carlos Silva",
            Cargo: "OperadorOperadorOperadorOperadorOperadorOperadorOperador",
            Incubatorio: "INC-001");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Cargo", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.CargoMuitoGrande.Replace("{1}", UsuarioLimites.CargoMax.ToString()),
            excecao.Errors["Cargo"]);
    }

    [Fact]
    public async Task Cadastrar_SemIncubatorio_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123456",
            Nome: "Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Incubatorio", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.NomeIncubatorioObrigatorio,
            excecao.Errors["Incubatorio"]);
    }

    [Fact]
    public async Task Cadastrar_ComIncubatorioCurto_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123456",
            Nome: "Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "IN");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Incubatorio", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.NomeIncubatorioCurto.Replace("{1}", UsuarioLimites.IncubatorioMin.ToString()),
            excecao.Errors["Incubatorio"]);
    }

    [Fact]
    public async Task Cadastrar_ComIncubatorioGrande_DeveRetornarBadRequest()
    {
        var request = new CadastrarUsuarioRequest(
            Usuario: "carlos",
            Senha: "123456",
            Nome: "Carlos Silva",
            Cargo: "Operador",
            Incubatorio: "IncubatorioIncubatorioIncubatorioIncubatorioIncubatorio");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/cadastrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Incubatorio", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarUsuarioErros.NomeIncubatorioMuitoGrande.Replace("{1}", UsuarioLimites.IncubatorioMax.ToString()),
            excecao.Errors["Incubatorio"]);
    }
}
