using Piu.Api.Features.Ovos.ReceberOvo;
using Piu.Api.Features.Ovos.ConsultarOvo;

namespace Piu.Api.Features.Ovos;

public static class OvoEndpoints
{
    public static IEndpointRouteBuilder MapOvos(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v1/ovos").WithTags("Ovos");
        ReceberOvoEndpoint.Map(grupo);
        ConsultarOvoEndpoint.Map(grupo);
        return app;
    }
}
