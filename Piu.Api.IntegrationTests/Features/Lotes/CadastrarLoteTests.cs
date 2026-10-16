using System.Net;
using System.Net.Http.Json;
using Piu.Api.Features.Lotes;
using Piu.Api.Features.Lotes.CadastrarLote;
using Piu.Api.IntegrationTests.Fixtures;

namespace Piu.Api.IntegrationTests.Features.Lotes;

[Collection("API")]
public sealed class CadastrarLoteTests(CustomWebApplicationFactory factory) : ApiIntegrationTestBase(factory)
{
    [Fact]
    public async Task CadastrarLote_ComDadosValidos_DeveRetornarCreated()
    {
        await AutenticarAsync();

        var request = new CadastrarLoteRequest(
            Nome: "DRD01-010",
            DataNascimento: "2026-03-14"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<CadastrarLoteResponse>();

        Assert.NotNull(content);
        Assert.Equal(request.Nome.ToUpperInvariant(), content.Nome);
        Assert.Equal(new DateOnly(2026, 3, 14), content.DataNascimento);
    }

    [Fact]
    public async Task CadastrarLote_ComNomeDuplicado_DeveRetornarConflict()
    {
        await AutenticarAsync();

        var request = new CadastrarLoteRequest(
            Nome: "DRD01-010",
            DataNascimento: "2026-03-14"
        );

        var primeiro = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            request);

        Assert.Equal(HttpStatusCode.Created, primeiro.StatusCode);

        var segundo = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            request);

        Assert.Equal(HttpStatusCode.Conflict, segundo.StatusCode);

        var erro = await segundo.Content.ReadFromJsonAsync<ApiErrorResponse>();

        Assert.NotNull(erro);
        Assert.Equal(CadastrarLoteErros.ErroNomeLoteEmUso.Code, erro.Code);
        Assert.Equal(CadastrarLoteErros.ErroNomeLoteEmUso.Message, erro.Detail);
        Assert.Equal(CadastrarLoteErros.ErroNomeLoteEmUso.StatusCode, erro.Status);
    }

    [Fact]
    public async Task CadastrarLote_SemNome_DeveRetornarBadRequest()
    {
        await AutenticarAsync();

        var request = new CadastrarLoteRequest(
            Nome: "",
            DataNascimento: "2026-03-14"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Nome", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarLoteErros.NomeObrigatorio,
            excecao.Errors["Nome"]);
    }

    [Fact]
    public async Task CadastrarLote_ComNomeCurto_DeveRetornarBadRequest()
    {
        await AutenticarAsync();

        var request = new CadastrarLoteRequest(
            Nome: "DR",
            DataNascimento: "2026-03-14"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Nome", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarLoteErros.NomeCurto.Replace("{1}", LoteLimites.NomeMin.ToString()),
            excecao.Errors["Nome"]);
    }

    [Fact]
    public async Task CadastrarLote_ComNomeMuitoGrande_DeveRetornarBadRequest()
    {
        await AutenticarAsync();

        var request = new CadastrarLoteRequest(
            Nome: "DRD01-010-NOME-DE-LOTE-MUITO-GRANDE",
            DataNascimento: "2026-03-14"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Nome", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarLoteErros.NomeMuitoGrande.Replace("{1}", LoteLimites.NomeMax.ToString()),
            excecao.Errors["Nome"]);
    }

    [Fact]
    public async Task CadastrarLote_SemDataNascimento_DeveRetornarBadRequest()
    {
        await AutenticarAsync();

        var request = new CadastrarLoteRequest(
            Nome: "DRD01-010",
            DataNascimento: null
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("DataNascimento", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarLoteErros.DataNascimentoObrigatoria,
            excecao.Errors["DataNascimento"]);
    }

    [Fact]
    public async Task CadastrarLote_ComDataNascimentoInvalida_DeveRetornarBadRequest()
    {
        await AutenticarAsync();

        var request = new CadastrarLoteRequest(
            Nome: "DRD01-010",
            DataNascimento: "14/03/2026"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var erro = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

        Assert.NotNull(erro);
        Assert.Equal(CadastrarLoteErros.ErroFormatoDataNascimento.Code, erro.Code);
        Assert.Equal(CadastrarLoteErros.ErroFormatoDataNascimento.Message, erro.Detail);
        Assert.Equal(CadastrarLoteErros.ErroFormatoDataNascimento.StatusCode, erro.Status);
    }

    [Fact]
    public async Task CadastrarLote_ComDataNascimentoFutura_DeveRetornarBadRequest()
    {
        await AutenticarAsync();

        var request = new CadastrarLoteRequest(
            Nome: "DRD01-010",
            DataNascimento: "3099-03-14"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/lotes",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var erro = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

        Assert.NotNull(erro);
        Assert.Equal(LoteErros.DataNascimentoInvalida.Code, erro.Code);
        Assert.Equal(LoteErros.DataNascimentoInvalida.Message, erro.Detail);
        Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)erro.Status);
    }

    private async Task AutenticarAsync()
    {
        var usuario = await Auth.CadastrarUsuarioLogadoAsync();
        ApiClient.DefaultRequestHeaders.Add("Cookie", usuario.Token);
    }
}
