using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Piu.Api.Features.Auth;

public sealed class UsuarioClaimsPrincipalFactory(
    UserManager<Usuario> userManager,
    IOptions<IdentityOptions> optionsAccessor)
    : UserClaimsPrincipalFactory<Usuario>(userManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("perfil", user.Perfil));

        if (!string.IsNullOrWhiteSpace(user.Incubatorio))
            identity.AddClaim(new Claim("incubatorio", user.Incubatorio));

        return identity;
    }
}
