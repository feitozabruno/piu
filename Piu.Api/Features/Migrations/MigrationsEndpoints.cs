using Microsoft.EntityFrameworkCore;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Migrations;

public static class MigrationsEndpoints
{
    public static IEndpointRouteBuilder MapMigrations(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v1/migrations")
            .WithTags("Migrations")
            .RequireAuthorization();

        grupo.MapGet("/", ConsultarAsync)
            .WithName("ConsultarMigrations")
            .WithSummary("Consulta o estado das migrations")
            .WithDescription("Retorna as migrations já executadas e as que ainda estão pendentes.")
            .Produces<ExecutarMigrationsResponse>(StatusCodes.Status200OK);

        grupo.MapPost("/", ExecutarAsync)
            .WithName("ExecutarMigrations")
            .WithSummary("Executa as migrations pendentes")
            .WithDescription("Aplica no banco de dados todas as migrations ainda não executadas.")
            .Produces<ExecutarMigrationsResponse>(StatusCodes.Status200OK);

        return app;
    }

    private static async Task<IResult> ConsultarAsync(
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        return Results.Ok(await ObterStatusAsync(db, cancellationToken));
    }

    private static async Task<IResult> ExecutarAsync(
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        await db.Database.MigrateAsync(cancellationToken);

        return Results.Ok(await ObterStatusAsync(db, cancellationToken));
    }

    private static async Task<ExecutarMigrationsResponse> ObterStatusAsync(
        AppDbContext db,
        CancellationToken cancellationToken)
    {
        var migrationsExecutadas = await db.Database
            .GetAppliedMigrationsAsync(cancellationToken);
        var migrationsPendentes = await db.Database
            .GetPendingMigrationsAsync(cancellationToken);

        return new ExecutarMigrationsResponse(
            [.. migrationsExecutadas],
            [.. migrationsPendentes]);
    }
}
