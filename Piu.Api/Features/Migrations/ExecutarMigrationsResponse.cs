namespace Piu.Api.Features.Migrations;

public sealed record ExecutarMigrationsResponse(
    IReadOnlyList<string> Executadas,
    IReadOnlyList<string> Pendentes
);
