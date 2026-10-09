using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Piu.Api.Application;

namespace Piu.Api.Features.Auth.FazerLogin;

public class FazerLoginHandler(
    UserManager<Usuario> userManager,
    SignInManager<Usuario> signInManager)
{
    public async Task<ClaimsPrincipal> HandleAsync(FazerLoginRequest request)
    {
        var usuario = await userManager.FindByNameAsync(request.Usuario);

        if (usuario is null || !usuario.Ativo)
        {
            throw new ApplicationErrorException(FazerLoginErros.CredenciaisInvalidas);
        }

        var resultado = await signInManager.CheckPasswordSignInAsync(
            usuario, request.Senha, lockoutOnFailure: true);

        if (resultado.IsLockedOut)
        {
            throw new ApplicationErrorException(FazerLoginErros.ContaBloqueada);
        }

        if (!resultado.Succeeded)
        {
            throw new ApplicationErrorException(FazerLoginErros.CredenciaisInvalidas);
        }

        var principal = await signInManager.CreateUserPrincipalAsync(usuario);
        return principal;
    }
}
