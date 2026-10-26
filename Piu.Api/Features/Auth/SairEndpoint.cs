using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Piu.Api.Features.Auth;

public static class SairEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/sair", SairAsync)
            .RequireAuthorization();
    }

    private static async Task SairAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        httpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}
