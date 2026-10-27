using Microsoft.EntityFrameworkCore;
using Piu.Api.Features.Incubatorios;
using Piu.Api.Features.Lotes;

namespace Piu.Api.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    private const string _incubatorioIdentificador = "180";
    private const string _incubatorioNome = "Incubatório Dourados";
    private const string _incubatorioSigla = "DRD";
    private static readonly DateOnly _dataNascimento = new(2026, 4, 14);
    private static readonly string[] _nomesLotes =
    [
        "DRD01-010",
        "DRD02-010",
        "DRD03-012",
        "DRD04-011",
        "DRD05-011",
        "DRD06-012",
        "DRD07-006",
        "DRD09-005",
        "DRD10-006"
    ];

    public static async Task SeedAsync(
        this IHost app,
        CancellationToken cancellationToken = default)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var incubatorio = await db.Incubatorios
            .SingleOrDefaultAsync(
                item => item.Identificador == _incubatorioIdentificador,
                cancellationToken);

        if (incubatorio is null)
        {
            incubatorio = Incubatorio.Criar(
                _incubatorioIdentificador,
                _incubatorioNome,
                _incubatorioSigla);

            db.Incubatorios.Add(incubatorio);
            await db.SaveChangesAsync(cancellationToken);
        }

        var lotesExistentes = await db.Lotes
            .Where(item =>
                item.IncubatorioIdentificador == _incubatorioIdentificador &&
                _nomesLotes.Contains(item.Nome))
            .Select(item => item.Nome)
            .ToListAsync(cancellationToken);

        var lotesParaInserir = _nomesLotes
            .Except(lotesExistentes, StringComparer.Ordinal)
            .Select(nome => Lote.Criar(
                nome,
                _dataNascimento,
                _incubatorioIdentificador))
            .ToList();

        if (lotesParaInserir.Count == 0) return;

        db.Lotes.AddRange(lotesParaInserir);
        await db.SaveChangesAsync(cancellationToken);
    }
}
