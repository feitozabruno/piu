using Piu.Api.Features.Auth;
using Piu.Api.Features.Incubatorios;
using Piu.Api.Features.Lotes;
using Piu.Api.Features.Migrations;
using Piu.Api.Features.Ovos;
using Piu.Api.Features.Relatorios;

namespace Piu.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static WebApplication MapPiuEndpoints(this WebApplication app)
    {
        app.MapAuth();
        app.MapIncubatorios();
        app.MapLotes();
        app.MapMigrations();
        app.MapOvos();
        app.MapRelatorios();

        app.MapGet("/", () => "Hello, World!");

        return app;
    }
}
