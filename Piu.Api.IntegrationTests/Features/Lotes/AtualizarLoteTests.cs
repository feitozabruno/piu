using System.Net;
using System.Net.Http.Json;
using Piu.Api.Features.Lotes.AtualizarLote;
using Piu.Api.Features.Lotes.CadastrarLote;
using Piu.Api.IntegrationTests.Fixtures;

namespace Piu.Api.IntegrationTests.Features.Lotes;

[Collection("API")]
public sealed class AtualizarLoteTests(CustomWebApplicationFactory factory) : ApiIntegrationTestBase(factory)
{
    [Fact]
    public async Task AtualizarLote_ComDadosValidos_DeveRetornarOk()
    {
        await AutenticarAsync();
        var criado = await CadastrarAsync("DRD01-010");

        var request = new AtualizarLoteRequest("DRD01-011", "2026-04-14", false);
        var response = await ApiClient.PutAsJsonAsync(
            $"/api/v1/lotes/{criado.Id}",
            request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<AtualizarLoteResponse>();
        Assert.NotNull(content);
        Assert.Equal(criado.Id, content.Id);
        Assert.Equal(request.Nome.ToUpperInvariant(), content.Nome);
        Assert.Equal(new DateOnly(2026, 4, 14), content.DataNascimento);
        Assert.False(content.Ativo);
    }

    [Fact]
    public async Task AtualizarLote_ComIdInexistente_DeveRetornarNotFound()
    {
        await AutenticarAsync();

        var response = await ApiClient.PutAsJsonAsync(
            $"/api/v1/lotes/{Guid.NewGuid()}",
            new AtualizarLoteRequest("DRD01-010", "2026-04-14", true));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AtualizarLote_ComNomeDuplicado_DeveRetornarConflict()
    {
        await AutenticarAsync();
        await CadastrarAsync("DRD01-010");
        var segundo = await CadastrarAsync("DRD01-011");

        var response = await ApiClient.PutAsJsonAsync(
            $"/api/v1/lotes/{segundo.Id}",
            new AtualizarLoteRequest("DRD01-010", "2026-04-14", true));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    private async Task<CadastrarLoteResponse> CadastrarAsync(string nome)
    {
        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            new CadastrarLoteRequest(nome, "2026-03-14"));

        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CadastrarLoteResponse>())!;
    }

    private async Task AutenticarAsync()
    {
        var usuario = await Auth.CadastrarUsuarioLogadoAsync();
        ApiClient.DefaultRequestHeaders.Add("Cookie", usuario.Token);
    }
}
