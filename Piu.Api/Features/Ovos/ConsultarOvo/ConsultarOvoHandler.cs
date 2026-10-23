using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Piu.Api.Exceptions;
using Piu.Api.Features.Auth;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Ovos.ConsultarOvo;

public sealed class ConsultarOvoHandler(
    AppDbContext db,
    UserManager<Usuario> userManager)
{
    public async Task<Ovo> HandleAsync(Guid id, ClaimsPrincipal usuarioAutenticado)
    {
        var usuario = await userManager.GetUserAsync(usuarioAutenticado)
            ?? throw new ApplicationErrorException(AuthErros.UsuarioNaoAutenticado);

        return await db.Ovos
            .Include(ovo => ovo.Lote)
            .SingleOrDefaultAsync(ovo =>
                ovo.Id == id
                && ovo.Lote.IncubatorioIdentificador == usuario.Incubatorio)
            ?? throw new ApplicationErrorException(ConsultarOvoErros.ErroNaoEncontrado);
    }
}
