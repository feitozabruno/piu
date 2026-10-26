using System.Globalization;
using System.IO.Compression;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using Piu.Api.Exceptions;
using Piu.Api.Features.Auth;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Relatorios;

public sealed class RelatorioOvosHandler(
    AppDbContext db,
    UserManager<Usuario> userManager,
    IWebHostEnvironment environment)
{
    private const double _firstRowY = 108;
    private const double _rowHeight = 20.25;
    private const double _textHeight = 9;
    private const double _fontSize = 9;

    public async Task<RelatorioOvosResult> GerarAsync(
        DateOnly dataInicial,
        DateOnly dataFinal,
        ClaimsPrincipal usuarioAutenticado,
        CancellationToken cancellationToken)
    {
        if (dataInicial == default || dataFinal == default || dataInicial > dataFinal)
            throw new ApplicationErrorException(RelatorioOvosErros.IntervaloInvalido);

        if (dataFinal.DayNumber - dataInicial.DayNumber + 1 > RelatorioOvosOptions.LimiteDias)
            throw new ApplicationErrorException(RelatorioOvosErros.IntervaloInvalido);

        var usuario = await userManager.GetUserAsync(usuarioAutenticado)
            ?? throw new ApplicationErrorException(AuthErros.UsuarioNaoAutenticado);

        var inicio = dataInicial.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var fim = dataFinal.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

        var registros = await db.Ovos
            .AsNoTracking()
            .Where(ovo =>
                ovo.Lote.IncubatorioIdentificador == usuario.Incubatorio
                && ovo.HoraRecebimento >= inicio
                && ovo.HoraRecebimento < fim)
            .OrderBy(ovo => ovo.HoraRecebimento)
            .Select(ovo => new RelatorioOvosResponse(
                ovo.HoraRecebimento,
                ovo.Lote.Nome,
                ovo.NinhoIncubavel,
                ovo.CamaIncubavel,
                ovo.TotalRecebido,
                ovo.TemperaturaRecebimento,
                ovo.TemperaturaBau,
                ovo.PlacaCaminhao))
            .ToListAsync(cancellationToken);

        var arquivos = new List<RelatorioOvosArquivo>();
        for (var data = dataInicial; data <= dataFinal; data = data.AddDays(1))
        {
            var registrosDoDia = registros
                .Where(registro => DateOnly.FromDateTime(registro.HoraRecebimento) == data)
                .ToList();

            if (registrosDoDia.Count == 0)
                continue;

            if (registrosDoDia.Count > RelatorioOvosOptions.RegistrosPorPagina)
                throw new ApplicationErrorException(RelatorioOvosErros.LimiteRegistrosExcedido);

            arquivos.Add(new RelatorioOvosArquivo(
                data,
                GerarPdf(data, registrosDoDia)));
        }

        return new RelatorioOvosResult(arquivos);
    }

    private byte[] GerarPdf(DateOnly data, IReadOnlyList<RelatorioOvosResponse> registros)
    {
        var caminhoFormulario = ObterCaminhoFormulario();

        if (!File.Exists(caminhoFormulario))
            throw new ApplicationErrorException(RelatorioOvosErros.FormularioIndisponivel);

        using var entrada = File.OpenRead(caminhoFormulario);
        using var documento = PdfReader.Open(entrada, PdfDocumentOpenMode.Modify);
        var pagina = documento.Pages[0];
        using (var grafico = XGraphics.FromPdfPage(pagina, XGraphicsPdfPageOptions.Append))
        {
            var fonte = new XFont("DejaVu Sans", _fontSize, XFontStyleEx.Regular);
            var pincel = new XSolidBrush(XColor.FromArgb(0, 0, 0));

            for (var indice = 0; indice < registros.Count; indice++)
            {
                var registro = registros[indice];
                var y = _firstRowY + indice * _rowHeight;

                DesenharTexto(grafico, fonte, pincel, data.ToString("dd/MM/yyyy"), 35, y, 62);
                DesenharTexto(grafico, fonte, pincel, registro.Lote, 105, y, 76);
                DesenharTexto(grafico, fonte, pincel, registro.NinhoIncubavel.ToString(CultureInfo.InvariantCulture), 290, y, 74);
                DesenharTexto(grafico, fonte, pincel, registro.CamaIncubavel.ToString(CultureInfo.InvariantCulture), 415, y, 74);
                DesenharTexto(grafico, fonte, pincel, registro.TemperaturaRecebimento.ToString("0.0", CultureInfo.InvariantCulture), 525, y, 60);
                DesenharTexto(grafico, fonte, pincel, registro.PlacaCaminhao, 613, y, 47);
                DesenharTexto(grafico, fonte, pincel, registro.HoraRecebimento.ToString("HH:mm"), 675, y, 39);
                DesenharTexto(grafico, fonte, pincel, registro.TemperaturaBau.ToString("0.##", CultureInfo.InvariantCulture), 725, y, 43);
            }
        }

        using var saida = new MemoryStream();
        documento.Save(saida, false);
        return saida.ToArray();
    }

    private string ObterCaminhoFormulario()
    {
        var caminhos = new[]
        {
            Path.Combine(
                AppContext.BaseDirectory,
                "Resources",
                RelatorioOvosOptions.NomeFormulario),
            Path.Combine(
                environment.ContentRootPath,
                "Resources",
                RelatorioOvosOptions.NomeFormulario)
        };

        return caminhos.FirstOrDefault(File.Exists) ?? caminhos[0];
    }

    private static void DesenharTexto(
        XGraphics grafico,
        XFont fonte,
        XBrush pincel,
        string texto,
        double x,
        double y,
        double largura)
    {
        var retangulo = new XRect(x, y, largura, _textHeight);
        grafico.DrawString(
            texto,
            fonte,
            pincel,
            retangulo,
            XStringFormats.TopLeft);
    }
}

public sealed record RelatorioOvosArquivo(DateOnly Data, byte[] Conteudo)
{
    public string Nome => $"controle-ovos-{Data:yyyy-MM-dd}.pdf";
}

public sealed record RelatorioOvosResult(IReadOnlyList<RelatorioOvosArquivo> Arquivos)
{
    public byte[] ComoZip()
    {
        using var saida = new MemoryStream();
        using (var zip = new ZipArchive(saida, ZipArchiveMode.Create, true))
        {
            foreach (var arquivo in Arquivos)
            {
                var entrada = zip.CreateEntry(arquivo.Nome, CompressionLevel.Fastest);
                using var stream = entrada.Open();
                stream.Write(arquivo.Conteudo);
            }
        }

        return saida.ToArray();
    }
}

public sealed class RelatorioOvosOptions
{
    public const int RegistrosPorPagina = 10;
    public const int LimiteDias = 31;
    public const string NomeFormulario = "3290.pdf";
}
