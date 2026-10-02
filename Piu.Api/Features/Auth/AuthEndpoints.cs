using Piu.Api.Features.Auth.CadastrarOperador;

namespace Piu.Api.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v1/auth").WithTags("Autenticação");
        CadastrarOperadorEndpoint.Map(grupo);

        return app;
    }
}
