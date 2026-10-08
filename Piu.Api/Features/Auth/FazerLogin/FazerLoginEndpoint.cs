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
        UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager)
    {
        var operador = await userManager.FindByNameAsync(request.Usuario);

        if (operador is null || !operador.Ativo)
            return CredenciaisInvalidas();

        var resultado = await signInManager.CheckPasswordSignInAsync(
            operador, request.Senha, lockoutOnFailure: true);

        if (resultado.IsLockedOut)
            return Results.Problem(
                detail: Mensagens.ContaBloqueada,
                statusCode: StatusCodes.Status429TooManyRequests
            );

        if (!resultado.Succeeded)
            return CredenciaisInvalidas();

        var principal = await signInManager.CreateUserPrincipalAsync(operador);

        return Results.SignIn(
            principal,
            new AuthenticationProperties { IsPersistent = true },
            IdentityConstants.ApplicationScheme);
    }

    private static IResult CredenciaisInvalidas() =>
        Results.Problem(
            detail: Mensagens.CredenciaisInvalidas,
            statusCode: StatusCodes.Status401Unauthorized);
}
