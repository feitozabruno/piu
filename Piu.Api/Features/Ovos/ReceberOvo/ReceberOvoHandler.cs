using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Piu.Api.Exceptions;
using Piu.Api.Features.Auth;
using Piu.Api.Features.Lotes;
using Piu.Api.Infrastructure.Persistence;

namespace Piu.Api.Features.Ovos.ReceberOvo;

public sealed class ReceberOvoHandler(AppDbContext db, UserManager<Usuario> userManager)
{
    public async Task<Ovo> HandleAsync(ReceberOvoRequest request, ClaimsPrincipal usuarioAutenticado)
    {
        if (request.ClientId == Guid.Empty)
            throw new ApplicationErrorException(ReceberOvoErros.ClientIdObrigatorio);

        var usuario = await ObterUsuarioAsync(usuarioAutenticado);
        var existente = await db.Ovos
            .Include(ovo => ovo.Lote)
            .SingleOrDefaultAsync(ovo =>
                ovo.ClientId == request.ClientId &&
                ovo.Lote.IncubatorioIdentificador == usuario.Incubatorio);

        if (existente is not null)
            return existente;

        var dataPostura = ParseDataPostura(request.DataPostura);
        var horaRecebimento = ParseHoraRecebimento(request.HoraRecebimento);

        var lote = await ObterLoteAsync(request.Lote, usuario.Incubatorio);

        var dados = CriarDados(request, dataPostura, horaRecebimento);
        var ovo = Ovo.Receber(lote, dados);

        db.Ovos.Add(ovo);
        await db.SaveChangesAsync();

        return ovo;
    }

    private static DateOnly ParseDataPostura(string? valor)
    {
        if (DateOnly.TryParseExact(
                valor,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var data))
        {
            if (data > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new ApplicationErrorException(ReceberOvoErros.ErroDataPosturaFutura);

            return data;
        }

        throw new ApplicationErrorException(
            ReceberOvoErros.ErroFormatoDataPostura);
    }

    private static DateTime ParseHoraRecebimento(string? valor)
    {
        if (DateTimeOffset.TryParseExact(
                valor,
                ["yyyy-MM-dd'T'HH:mm:ssK", "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK"],
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var hora))
        {
            if (hora.UtcDateTime > DateTime.UtcNow.AddHours(2))
                throw new ApplicationErrorException(ReceberOvoErros.ErroHoraRecebimentoFutura);

            return hora.UtcDateTime;
        }

        throw new ApplicationErrorException(
            ReceberOvoErros.ErroFormatoHoraRecebimento);
    }

    private async Task<Usuario> ObterUsuarioAsync(ClaimsPrincipal usuarioAutenticado)
    {
        return await userManager.GetUserAsync(usuarioAutenticado)
            ?? throw new ApplicationErrorException(
                AuthErros.UsuarioNaoAutenticado);
    }

    private async Task<Lote> ObterLoteAsync(string nome, string incubatorio)
    {
        return await db.Lotes.SingleOrDefaultAsync(lote =>
            lote.Nome == nome.Trim().ToUpperInvariant() &&
            lote.IncubatorioIdentificador == incubatorio)
            ?? throw new ApplicationErrorException(
                ReceberOvoErros.ErroLoteNaoEncontrado);
    }

    private static OvoRecebimentoDados CriarDados(
        ReceberOvoRequest request,
        DateOnly dataPostura,
        DateTime horaRecebimento)
    {
        return new OvoRecebimentoDados
        {
            ClientId = request.ClientId,
            DataPostura = dataPostura,
            NinhoIncubavel = request.NinhoIncubavel,
            CamaIncubavel = request.CamaIncubavel,
            TemperaturaRecebimento = request.TemperaturaOvo,
            TemperaturaBau = request.TemperaturaBau,
            PlacaCaminhao = request.PlacaCaminhao,
            Motorista = request.Motorista,
            HoraRecebimento = horaRecebimento,
            Observacao = request.Observacao
        };
    }
}
