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
    public async Task<IReadOnlyList<Ovo>> ListarAsync(
        DateOnly? dataInicial,
        DateOnly? dataFinal,
        ClaimsPrincipal usuarioAutenticado,
        CancellationToken cancellationToken)
    {
        var usuario = await userManager.GetUserAsync(usuarioAutenticado)
            ?? throw new ApplicationErrorException(AuthErros.UsuarioNaoAutenticado);

        var consulta = db.Ovos
            .AsNoTracking()
            .Include(ovo => ovo.Lote)
            .Where(ovo => ovo.Lote.IncubatorioIdentificador == usuario.Incubatorio);

        if (dataInicial is not null)
        {
            var inicio = dataInicial.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            consulta = consulta.Where(ovo => ovo.HoraRecebimento >= inicio);
        }

        if (dataFinal is not null)
        {
            var fim = dataFinal.Value.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            consulta = consulta.Where(ovo => ovo.HoraRecebimento < fim);
        }

        return await consulta
            .OrderByDescending(ovo => ovo.HoraRecebimento)
            .ToListAsync(cancellationToken);
    }

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
