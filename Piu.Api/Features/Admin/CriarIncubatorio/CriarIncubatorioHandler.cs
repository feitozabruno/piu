using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Piu.Api.Exceptions;
using Piu.Api.Features.Auth;
using Piu.Api.Features.Incubatorios;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Admin.CriarIncubatorio;

public sealed class CriarIncubatorioHandler(
    AppDbContext db,
    UserManager<Usuario> userManager)
{
    public async Task<(Incubatorio Incubatorio, Usuario Gerente)> HandleAsync(
        CriarIncubatorioRequest request,
        CancellationToken cancellationToken)
    {
        var incubatorio = Incubatorio.Criar(request.Identificador, request.Nome, request.Sigla);
        var gerente = Usuario.Criar(
            request.UsuarioGerente,
            request.NomeGerente,
            "Gerente",
            incubatorio.Identificador,
            Usuario.Gerente);

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        db.Incubatorios.Add(incubatorio);

        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new ApplicationErrorException(AdminErrors.IncubatorioEmUso);
        }

        var resultado = await userManager.CreateAsync(gerente, request.SenhaGerente);
        if (!resultado.Succeeded)
        {
            if (resultado.Errors.Any(e => e.Code == "DuplicateUserName"))
                throw new ApplicationErrorException(AdminErrors.UsuarioEmUso);

            throw new ApplicationErrorException(AdminErrors.FalhaAoCriarUsuario);
        }

        await transaction.CommitAsync(cancellationToken);
        return (incubatorio, gerente);
    }
}
