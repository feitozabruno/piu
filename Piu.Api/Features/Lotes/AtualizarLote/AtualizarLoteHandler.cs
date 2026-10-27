using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Piu.Api.Exceptions;
using Piu.Api.Features.Auth;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Lotes.AtualizarLote;

public sealed class AtualizarLoteHandler(
    AppDbContext db,
    UserManager<Usuario> userManager)
{
    public async Task<Lote> HandleAsync(
        Guid id,
        AtualizarLoteRequest request,
        ClaimsPrincipal usuarioAutenticado)
    {
        if (!DateOnly.TryParseExact(
                request.DataNascimento,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var dataNascimento))
        {
            throw new ApplicationErrorException(
                AtualizarLoteErros.ErroFormatoDataNascimento);
        }

        var usuario = await userManager.GetUserAsync(usuarioAutenticado)
            ?? throw new ApplicationErrorException(
                AuthErros.UsuarioNaoAutenticado);

        var lote = await db.Lotes.SingleOrDefaultAsync(item =>
            item.Id == id &&
            item.IncubatorioIdentificador == usuario.Incubatorio)
            ?? throw new ApplicationErrorException(
                AtualizarLoteErros.ErroLoteNaoEncontrado);

        lote.Atualizar(request.Nome, dataNascimento, request.Ativo);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: "IX_Lotes_IncubatorioIdentificador_Nome"
            })
        {
            throw new ApplicationErrorException(
                AtualizarLoteErros.ErroNomeLoteEmUso);
        }

        return lote;
    }
}
