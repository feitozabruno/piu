using PdfSharp.Fonts;

namespace Piu.Api.Features.Relatorios;

public sealed class RelatorioFontResolver : IFontResolver
{
    public const string FontKey = "DejaVuSans#";
    public string DefaultFontName => FontKey;

    private static readonly byte[] _fontData = LoadFont();

    public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        return familyName.Equals("DejaVu Sans", StringComparison.OrdinalIgnoreCase)
            ? new FontResolverInfo(FontKey)
            : null;
    }

    public byte[]? GetFont(string faceName) =>
        faceName == FontKey ? _fontData : null;

    private static byte[] LoadFont()
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Resources",
            "Fonts",
            "DejaVuSans.ttf");

        if (!File.Exists(path))
            throw new FileNotFoundException(
                "A fonte do relatório não está disponível.",
                path);

        return File.ReadAllBytes(path);
    }
}
