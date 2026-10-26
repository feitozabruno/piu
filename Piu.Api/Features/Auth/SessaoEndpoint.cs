namespace Piu.Api.Features.Auth;

public static class SessaoEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/sessao", ObterAsync)
            .AllowAnonymous();
    }

    private static IResult ObterAsync(HttpContext httpContext)
    {
        var autenticado = httpContext.User.Identity?.IsAuthenticated == true;

        return Results.Ok(new { autenticado });
    }
}
