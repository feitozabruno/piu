using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Piu.Api.Features.Auth;

public static class SessaoEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/sessao", ObterAsync)
            .AllowAnonymous();
    }

    private static async Task<IResult> ObterAsync(
        HttpContext httpContext,
        UserManager<Usuario> userManager)
    {
        var autenticado = httpContext.User.Identity?.IsAuthenticated == true;
        var usuario = autenticado
            ? await userManager.GetUserAsync(httpContext.User)
            : null;

        return Results.Ok(new
        {
            autenticado = usuario is not null,
            perfil = usuario?.Perfil,
            incubatorio = usuario?.Incubatorio
        });
    }
}
