using System.Net;
using System.Net.Http.Json;
using Piu.Api.Features.Lotes.CadastrarLote;
using Piu.Api.Features.Ovos.ReceberOvo;
using Piu.Api.IntegrationTests.Fixtures;

namespace Piu.Api.IntegrationTests.Features.Ovos;

[Collection("API")]
public sealed class ReceberOvoTests(CustomWebApplicationFactory factory)
    : ApiIntegrationTestBase(factory)
{
    [Fact]
    public async Task ReceberOvo_ComDadosValidos_DevePermitirConsultarPeloLocation()
    {
        await AutenticarAsync();
        await CriarLoteAsync();

        var request = CriarRequest();
        var response = await ApiClient.PostAsJsonAsync("/api/v1/ovos", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var criado = await response.Content.ReadFromJsonAsync<ReceberOvoResponse>();
        Assert.NotNull(criado);
        Assert.Equal(33, criado.TotalRecebido);

        var consulta = await ApiClient.GetAsync(response.Headers.Location);

        Assert.Equal(HttpStatusCode.OK, consulta.StatusCode);
        var consultado = await consulta.Content.ReadFromJsonAsync<ReceberOvoResponse>();
        Assert.Equal(criado, consultado);
    }

    [Fact]
    public async Task ConsultarOvo_ComIdInexistente_DeveRetornarNotFound()
    {
        await AutenticarAsync();

        var consulta = await ApiClient.GetAsync($"/api/v1/ovos/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, consulta.StatusCode);
    }

    private async Task AutenticarAsync()
    {
        var usuario = await Auth.CadastrarUsuarioLogadoAsync();
        ApiClient.DefaultRequestHeaders.Add("Cookie", usuario.Token);
    }

    private async Task CriarLoteAsync()
    {
        var request = new CadastrarLoteRequest("DRD01-010", "2026-03-14");
        var response = await ApiClient.PostAsJsonAsync("/api/v1/lotes", request);
        response.EnsureSuccessStatusCode();
    }

    private static ReceberOvoRequest CriarRequest() => new(
        " drd01-010 ",
        "2026-10-13",
        30,
        3,
        18.00m,
        18.50m,
        " ABC1234 ",
        " Maria ",
        "2026-10-14T09:30:00Z",
        " Recebido em bom estado. ");
}
