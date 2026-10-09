using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Piu.Api.Features.Auth.FazerLogin;

public static class FazerLoginEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/entrar", EntrarAsync);
    }

    private static async Task<IResult> EntrarAsync(
        FazerLoginRequest request,
        FazerLoginHandler handler)
    {
        var login = await handler.HandleAsync(request);

        return Results.SignIn(
            login,
            new AuthenticationProperties { IsPersistent = true },
            IdentityConstants.ApplicationScheme);
    }
}
