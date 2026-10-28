using System.Net;
using System.Net.Http.Json;
using Piu.Api.Features.Migrations;
using Piu.Api.IntegrationTests.Fixtures;

namespace Piu.Api.IntegrationTests.Features.Migrations;

[Collection("API")]
public sealed class MigrationsTests(CustomWebApplicationFactory factory) : ApiIntegrationTestBase(factory)
{
    [Fact]
    public async Task ExecutarMigrations_SemAutenticacao_DeveRetornarNaoAutorizado()
    {
        var response = await ApiClient.PostAsync("/api/v1/migrations", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ConsultarMigrations_ComUsuarioAutenticado_DeveRetornarExecutadasEPendentes()
    {
        var usuario = await Auth.CadastrarUsuarioLogadoAsync();
        ApiClient.DefaultRequestHeaders.Add("Cookie", usuario.Token);

        var response = await ApiClient.GetAsync("/api/v1/migrations");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var resultado = await response.Content
            .ReadFromJsonAsync<ExecutarMigrationsResponse>();

        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado.Executadas);
        Assert.Empty(resultado.Pendentes);
    }

    [Fact]
    public async Task ExecutarMigrations_ComUsuarioAutenticado_DeveRetornarStatusAtualizado()
    {
        var usuario = await Auth.CadastrarUsuarioLogadoAsync();
        ApiClient.DefaultRequestHeaders.Add("Cookie", usuario.Token);

        var response = await ApiClient.PostAsync("/api/v1/migrations", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var resultado = await response.Content
            .ReadFromJsonAsync<ExecutarMigrationsResponse>();

        Assert.NotNull(resultado);
        Assert.NotEmpty(resultado.Executadas);
        Assert.Empty(resultado.Pendentes);
    }
}
