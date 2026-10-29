using System.Net.Http.Json;
using Microsoft.Net.Http.Headers;
using Piu.Api.Features.Auth.CadastrarUsuario;
using Piu.Api.Features.Auth.FazerLogin;

namespace Piu.Api.Tests.Integration.Fixtures;

public sealed class AuthHelper(HttpClient client)
{
    private readonly HttpClient _client = client;
    private static string UniqueUsername() => $"{Guid.NewGuid():N}";
    private const string _defaultPassword = "SenhaTeste123";

    public async Task<UsuarioCadastrado> CadastrarUsuarioAsync(string usuario = "", string senha = "")
    {
        if (string.IsNullOrEmpty(usuario)) usuario = UniqueUsername();
        if (string.IsNullOrEmpty(senha)) senha = _defaultPassword;

        var request = new CadastrarUsuarioRequest(
            Usuario: usuario,
            Senha: senha,
            Nome: "Teste",
            Cargo: "Operador",
            Incubatorio: "INC-001"
        );

        var response = await _client.PostAsJsonAsync("/api/v1/auth/cadastrar", request);
        response.EnsureSuccessStatusCode();

        return new UsuarioCadastrado(usuario, senha);
    }

    public async Task<UsuarioLogado> CadastrarUsuarioLogadoAsync(string usuario = "", string senha = "")
    {
        if (string.IsNullOrEmpty(usuario)) usuario = UniqueUsername();
        if (string.IsNullOrEmpty(senha)) senha = _defaultPassword;

        var novoUsuario = await CadastrarUsuarioAsync(usuario, senha);

        var request = new FazerLoginRequest(
            Usuario: novoUsuario.Usuario,
            Senha: novoUsuario.Senha
        );

        var response = await _client.PostAsJsonAsync("/api/v1/auth/entrar", request);
        response.EnsureSuccessStatusCode();

        var rawCookie = response.Headers.GetValues("Set-Cookie").First();
        var cookie = SetCookieHeaderValue.Parse(rawCookie);
        var cookieHeaderValue = new CookieHeaderValue(cookie.Name, cookie.Value).ToString();

        return new UsuarioLogado(
            Usuario: novoUsuario.Usuario,
            Senha: senha,
            Token: cookieHeaderValue
        );
    }
}

public record UsuarioCadastrado(string Usuario, string Senha);
public record UsuarioLogado(string Usuario, string Senha, string Token);
