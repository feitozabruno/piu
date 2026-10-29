using System.Net;
using System.Net.Http.Json;
using Piu.Api.Features.Auth.FazerLogin;
using Piu.Api.Tests.Integration.Fixtures;

namespace Piu.Api.Tests.Integration.Features.Auth;

[Collection("API")]
public sealed class FazerLoginEndpointTests(CustomWebApplicationFactory factory) : ApiIntegrationTestBase(factory)
{
    [Fact]
    public async Task FazerLogin_ComUsuarioCadastrado_DeveCriarCookieDeSessao()
    {
        var novoUsuario = await Auth.CadastrarUsuarioAsync(usuario: "maria", senha: "senha123");

        var request = new FazerLoginRequest(
            Usuario: novoUsuario.Usuario,
            Senha: novoUsuario.Senha
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/entrar",
            request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        Assert.Empty(content);

        var cookies = response.Headers.GetValues("Set-Cookie");

        Assert.Contains(
            cookies,
            cookie => cookie.StartsWith("piu.session="));

        Assert.Contains(
            cookies,
            cookie => cookie.Contains("httponly", StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            cookies,
            cookie => cookie.Contains("secure", StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            cookies,
            cookie => cookie.Contains("samesite=none", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task FazerLogin_ComUsuarioGerado_DeveCriarCookieDeSessao()
    {
        var novoUsuario = await Auth.CadastrarUsuarioAsync();

        var request = new FazerLoginRequest(
            Usuario: novoUsuario.Usuario,
            Senha: novoUsuario.Senha
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/entrar",
            request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        Assert.Empty(content);

        var cookies = response.Headers.GetValues("Set-Cookie");

        Assert.Contains(
            cookies,
            cookie => cookie.StartsWith("piu.session="));

        Assert.Contains(
            cookies,
            cookie => cookie.Contains("httponly", StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            cookies,
            cookie => cookie.Contains("secure", StringComparison.OrdinalIgnoreCase));

        Assert.Contains(
            cookies,
            cookie => cookie.Contains("samesite=none", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task FazerLogin_SemUsuario_DeveRetornarBadRequest()
    {
        var request = new FazerLoginRequest(
            Usuario: "",
            Senha: "123456");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/entrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Usuario", excecao.Errors.Keys);
        Assert.Contains(
            FazerLoginErros.UsuarioObrigatorio,
            excecao.Errors["Usuario"]);
    }

    [Fact]
    public async Task FazerLogin_SemSenha_DeveRetornarBadRequest()
    {
        var request = new FazerLoginRequest(
            Usuario: "joao",
            Senha: "");

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/entrar",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Senha", excecao.Errors.Keys);
        Assert.Contains(
            FazerLoginErros.SenhaObrigatoria,
            excecao.Errors["Senha"]);
    }

    [Fact]
    public async Task FazerLogin_ComCredenciaisInvalidas_DeveRetornarUnauthorized()
    {
        var request = new FazerLoginRequest(
            Usuario: "usuarioInexistente",
            Senha: "senhaIncorreta"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/auth/entrar",
            request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

        Assert.NotNull(excecao);

        Assert.Equal(
            FazerLoginErros.CredenciaisInvalidas.Code,
            excecao.Code);

        Assert.Equal(
            FazerLoginErros.CredenciaisInvalidas.Message,
            excecao.Detail);

        Assert.Equal(
            FazerLoginErros.CredenciaisInvalidas.StatusCode,
            excecao.Status);
    }
}
