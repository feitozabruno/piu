using Piu.Api.Features.Auth;
using Piu.Api.Features.Admin;
using Piu.Api.Features.Incubatorios;
using Piu.Api.Features.Lotes;
using Piu.Api.Features.Ovos;

namespace Piu.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static WebApplication MapPiuEndpoints(this WebApplication app)
    {
        app.MapAuth();
        app.MapAdmin();
        app.MapIncubatorios();
        app.MapLotes();
        app.MapOvos();

        app.MapGet("/", () => "Hello, World!");

        return app;
    }
}
