using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Piu.Api.Application;
using Piu.Api.Features.Auth;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Lotes.ListarLotes;

public sealed class ListarLotesHandler(
    AppDbContext db,
    UserManager<Usuario> userManager)
{
    public async Task<IReadOnlyList<ListarLotesResponse>> HandleAsync(
        ClaimsPrincipal usuarioAutenticado)
    {
        var usuario = await userManager.GetUserAsync(usuarioAutenticado)
            ?? throw new ApplicationErrorException(AuthErros.UsuarioNaoAutenticado);

        return await db.Lotes
            .AsNoTracking()
            .Where(lote => lote.IncubatorioIdentificador == usuario.Incubatorio)
            .OrderBy(lote => lote.Nome)
            .Select(lote => new ListarLotesResponse(
                lote.Id,
                lote.Nome,
                lote.DataNascimento))
            .ToListAsync();
    }
}
