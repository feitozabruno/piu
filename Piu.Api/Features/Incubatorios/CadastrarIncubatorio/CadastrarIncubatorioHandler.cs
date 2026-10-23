using Microsoft.EntityFrameworkCore;
using Npgsql;
using Piu.Api.Exceptions;
using Piu.Api.Infrastructure.Persistence;
using Piu.Api.Infrastructure.Persistence.Configurations;

namespace Piu.Api.Features.Incubatorios.CadastrarIncubatorio;

public class CadastrarIncubatorioHandler(AppDbContext db)
{
    public async Task<Incubatorio> HandleAsync(
        CadastrarIncubatorioRequest request)
    {
        var incubatorio = Incubatorio.Criar(
            request.Identificador,
            request.Nome,
            request.Sigla);

        db.Incubatorios.Add(incubatorio);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: IncubatorioConstraints.PkIdentificador
            })
        {
            throw new ApplicationErrorException(
                CadastrarIncubatorioErros.ErroIdentificadorEmUso);
        }

        return incubatorio;
    }
}
