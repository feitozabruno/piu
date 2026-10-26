using Piu.Api.Features.Auth.CadastrarUsuario;
using Piu.Api.Features.Auth.FazerLogin;
using Piu.Api.Features.Incubatorios.CadastrarIncubatorio;
using Piu.Api.Features.Lotes.CadastrarLote;
using Piu.Api.Features.Lotes.ListarLotes;
using Piu.Api.Features.Ovos.ConsultarOvo;
using Piu.Api.Features.Ovos.ReceberOvo;
using Piu.Api.Features.Relatorios;

namespace Piu.Api.Extensions;

public static class HandlerCollectionExtensions
{
    public static IServiceCollection AddFeatureHandlers(this IServiceCollection services)
    {
        services.AddScoped<CadastrarUsuarioHandler>();
        services.AddScoped<FazerLoginHandler>();
        services.AddScoped<CadastrarIncubatorioHandler>();
        services.AddScoped<CadastrarLoteHandler>();
        services.AddScoped<ListarLotesHandler>();
        services.AddScoped<ReceberOvoHandler>();
        services.AddScoped<ConsultarOvoHandler>();
        services.AddScoped<RelatorioOvosHandler>();

        return services;
    }
}
