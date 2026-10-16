using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Piu.Api.Application;
using Piu.Api.Features.Auth;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Lotes.CadastrarLote;

public class CadastrarLoteHandler(
    AppDbContext db,
    UserManager<Usuario> userManager)
{
    public async Task<Lote> HandleAsync(
        CadastrarLoteRequest request,
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
                CadastrarLoteErros.ErroFormatoDataNascimento);
        }

        var usuario = await userManager.GetUserAsync(usuarioAutenticado)
            ?? throw new ApplicationErrorException(CadastrarLoteErros.UsuarioNaoAutenticado);

        var lote = Lote.Criar(
            request.Nome,
            dataNascimento,
            usuario.Incubatorio);

        db.Lotes.Add(lote);

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
                CadastrarLoteErros.ErroNomeLoteEmUso);
        }

        return lote;
    }
}
