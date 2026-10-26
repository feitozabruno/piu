using Microsoft.EntityFrameworkCore;
using Piu.Api.Features.Auth;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Admin.ListarIncubatorios;

public static class ListarIncubatoriosEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/incubatorios", async (
                AppDbContext db,
                CancellationToken cancellationToken) =>
            {
                var incubatorios = await db.Incubatorios
                    .AsNoTracking()
                    .OrderBy(i => i.Nome)
                    .Select(i => new
                    {
                        i.Identificador,
                        i.Nome,
                        i.Sigla,
                        i.Ativo,
                        i.DataCadastro,
                        Gerente = db.Users
                            .Where(u => u.Incubatorio == i.Identificador && u.Perfil == Usuario.Gerente)
                            .Select(u => new { u.UserName, u.Nome, u.Ativo })
                            .FirstOrDefault()
                    })
                    .ToListAsync(cancellationToken);

                return Results.Ok(incubatorios);
            })
            .RequireAuthorization(Usuario.Administrador)
            .WithName("ListarIncubatoriosAdministracao");
    }
}
