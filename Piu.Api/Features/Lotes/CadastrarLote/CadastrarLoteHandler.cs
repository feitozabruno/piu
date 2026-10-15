using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Piu.Api.Application;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Lotes.CadastrarLote;

public class CadastrarLoteHandler(AppDbContext db)
{
    public async Task<Lote> HandleAsync(
        CadastrarLoteRequest request)
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

        var lote = Lote.Criar(
            request.Nome,
            dataNascimento);

        db.Lotes.Add(lote);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: "IX_Lotes_Nome"
            })
        {
            throw new ApplicationErrorException(
                CadastrarLoteErros.ErroNomeLoteEmUso);
        }

        return lote;
    }
}
