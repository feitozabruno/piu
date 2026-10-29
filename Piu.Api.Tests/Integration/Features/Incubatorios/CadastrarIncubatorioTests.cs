using System.Net;
using System.Net.Http.Json;
using Piu.Api.Features.Incubatorios;
using Piu.Api.Features.Incubatorios.CadastrarIncubatorio;
using Piu.Api.Tests.Integration.Fixtures;

namespace Piu.Api.Tests.Integration.Features.Incubatorios;

[Collection("API")]
public sealed class CadastrarIncubatorioTests(CustomWebApplicationFactory factory) : ApiIntegrationTestBase(factory)
{
    [Fact]
    public async Task CadastrarIncubatorio_ComDadosValidos_DeveRetornarCreated()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "180",
            Nome: "Incubatório de Dourados",
            Sigla: "DRD"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<CadastrarIncubatorioResponse>();

        Assert.NotNull(content);
        Assert.Equal(request.Identificador, content.Identificador);
        Assert.Equal(request.Nome, content.Nome);
        Assert.Equal(request.Sigla, content.Sigla);
    }

    [Fact]
    public async Task CadastrarIncubatorio_ComIdentificadorDuplicado_DeveRetornarConflict()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "180",
            Nome: "Incubatório de Dourados",
            Sigla: "DRD"
        );

        var primeiro = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.Created, primeiro.StatusCode);

        var segundo = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.Conflict, segundo.StatusCode);

        var erro = await segundo.Content.ReadFromJsonAsync<ApiErrorResponse>();

        Assert.NotNull(erro);
        Assert.Equal(CadastrarIncubatorioErros.ErroIdentificadorEmUso.Code, erro.Code);
        Assert.Equal(CadastrarIncubatorioErros.ErroIdentificadorEmUso.Message, erro.Detail);
        Assert.Equal(CadastrarIncubatorioErros.ErroIdentificadorEmUso.StatusCode, erro.Status);
    }

    [Fact]
    public async Task CadastrarIncubatorio_SemIdentificador_DeveRetornarBadRequest()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "",
            Nome: "Incubatório de Dourados",
            Sigla: "DRD"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Identificador", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarIncubatorioErros.IdentificadorObrigatorio,
            excecao.Errors["Identificador"]);
    }

    [Fact]
    public async Task CadastrarIncubatorio_ComIdentificadorCurto_DeveRetornarBadRequest()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "1",
            Nome: "Incubatório de Dourados",
            Sigla: "DRD"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Identificador", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarIncubatorioErros.IdentificadorCurto.Replace("{1}", IncubatorioLimites.IdentificadorMin.ToString()),
            excecao.Errors["Identificador"]);
    }

    [Fact]
    public async Task CadastrarIncubatorio_ComIdentificadorMuitoGrande_DeveRetornarBadRequest()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "123456789012345678901234567890123456789012345678901",
            Nome: "Incubatório de Dourados",
            Sigla: "DRD"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Identificador", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarIncubatorioErros.IdentificadorMuitoGrande.Replace("{1}", IncubatorioLimites.IdentificadorMax.ToString()),
            excecao.Errors["Identificador"]);
    }

    [Fact]
    public async Task CadastrarIncubatorio_SemNome_DeveRetornarBadRequest()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "180",
            Nome: "",
            Sigla: "DRD"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Nome", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarIncubatorioErros.NomeObrigatorio,
            excecao.Errors["Nome"]);
    }

    [Fact]
    public async Task CadastrarIncubatorio_ComNomeCurto_DeveRetornarBadRequest()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "180",
            Nome: "I",
            Sigla: "DRD"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Nome", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarIncubatorioErros.NomeCurto.Replace("{1}", IncubatorioLimites.NomeMin.ToString()),
            excecao.Errors["Nome"]);
    }

    [Fact]
    public async Task CadastrarIncubatorio_ComNomeMuitoGrande_DeveRetornarBadRequest()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "180",
            Nome: "Incubatório de Dourados com um nome muito grande que ultrapassa o limite máximo de caracteres permitido para o nome do incubatório, que é de 150 caracteres",
            Sigla: "DRD"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Nome", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarIncubatorioErros.NomeMuitoGrande.Replace("{1}", IncubatorioLimites.NomeMax.ToString()),
            excecao.Errors["Nome"]);
    }

    [Fact]
    public async Task CadastrarIncubatorio_SemSigla_DeveRetornarBadRequest()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "180",
            Nome: "Incubatório de Dourados",
            Sigla: ""
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Sigla", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarIncubatorioErros.SiglaObrigatoria,
            excecao.Errors["Sigla"]);
    }

    [Fact]
    public async Task CadastrarIncubatorio_ComSiglaCurta_DeveRetornarBadRequest()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "180",
            Nome: "Incubatório de Dourados",
            Sigla: "D"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Sigla", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarIncubatorioErros.SiglaCurta.Replace("{1}", IncubatorioLimites.SiglaMin.ToString()),
            excecao.Errors["Sigla"]);
    }

    [Fact]
    public async Task CadastrarIncubatorio_ComSiglaMuitoGrande_DeveRetornarBadRequest()
    {
        var request = new CadastrarIncubatorioRequest(
            Identificador: "180",
            Nome: "Incubatório de Dourados",
            Sigla: "DRD-INCUBATORIO-DE-DOURADOS"
        );

        var response = await ApiClient.PostAsJsonAsync(
            "/api/v1/incubatorios",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var excecao = await response.Content.ReadFromJsonAsync<ApiValidationErrorResponse>();

        Assert.NotNull(excecao);
        Assert.Contains("Sigla", excecao.Errors.Keys);
        Assert.Contains(
            CadastrarIncubatorioErros.SiglaMuitoGrande.Replace("{1}", IncubatorioLimites.SiglaMax.ToString()),
            excecao.Errors["Sigla"]);
    }
}
