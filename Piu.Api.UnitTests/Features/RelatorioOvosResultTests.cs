using System.IO.Compression;
using Piu.Api.Features.Relatorios;

namespace Piu.Api.UnitTests.Features;

public sealed class RelatorioOvosResultTests
{
    [Fact]
    public void ComoZip_deve_conter_um_arquivo_por_data()
    {
        var resultado = new RelatorioOvosResult(
        [
            new RelatorioOvosArquivo(new DateOnly(2026, 10, 26), [1, 2]),
            new RelatorioOvosArquivo(new DateOnly(2026, 10, 27), [3, 4])
        ]);

        using var zip = new ZipArchive(
            new MemoryStream(resultado.ComoZip()),
            ZipArchiveMode.Read);

        Assert.Equal(
            ["controle-ovos-2026-10-26.pdf", "controle-ovos-2026-10-27.pdf"],
            zip.Entries.Select(entrada => entrada.FullName));
    }

    [Fact]
    public void Arquivo_deve_usar_nome_seguro_e_identificavel()
    {
        var arquivo = new RelatorioOvosArquivo(
            new DateOnly(2026, 10, 26),
            []);

        Assert.Equal("controle-ovos-2026-10-26.pdf", arquivo.Nome);
    }
}
