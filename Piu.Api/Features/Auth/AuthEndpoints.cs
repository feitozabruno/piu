using Piu.Api.Features.Auth.CadastrarOperador;
using Piu.Api.Features.Auth.FazerLogin;

namespace Piu.Api.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v1/auth").WithTags("Autenticação");
        CadastrarOperadorEndpoint.Map(grupo);
        FazerLoginEndpoint.Map(grupo);

        return app;
    }
}
