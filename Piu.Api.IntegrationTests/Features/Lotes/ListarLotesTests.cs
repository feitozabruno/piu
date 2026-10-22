using System.Net;
using System.Net.Http.Json;
using Piu.Api.Features.Lotes.CadastrarLote;
using Piu.Api.Features.Lotes.ListarLotes;
using Piu.Api.IntegrationTests.Fixtures;

namespace Piu.Api.IntegrationTests.Features.Lotes;

[Collection("API")]
public sealed class ListarLotesTests(CustomWebApplicationFactory factory) : ApiIntegrationTestBase(factory)
{
    [Fact]
    public async Task ListarLotes_ComUsuarioAutenticado_DeveRetornarLotesDoIncubatorio()
    {
        await AutenticarAsync();
        var request = new CadastrarLoteRequest("DRD01-010", "2026-03-14");
        await ApiClient.PostAsJsonAsync("/api/v1/lotes", request);

        var response = await ApiClient.GetAsync("/api/v1/lotes");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var lotes = await response.Content.ReadFromJsonAsync<List<ListarLotesResponse>>();
        Assert.NotNull(lotes);
        Assert.Contains(lotes, lote => lote.Nome == request.Nome);
    }

    private async Task AutenticarAsync()
    {
        var usuario = await Auth.CadastrarUsuarioLogadoAsync();
        ApiClient.DefaultRequestHeaders.Add("Cookie", usuario.Token);
    }
}
