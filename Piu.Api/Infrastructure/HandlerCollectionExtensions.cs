using Piu.Api.Features.Auth.CadastrarUsuario;
using Piu.Api.Features.Auth.FazerLogin;
using Piu.Api.Features.Incubatorios.CadastrarIncubatorio;
using Piu.Api.Features.Lotes.CadastrarLote;
using Piu.Api.Features.Ovos.ConsultarOvo;
using Piu.Api.Features.Ovos.ReceberOvo;

namespace Piu.Api.Infrastructure;

public static class HandlerCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<CadastrarUsuarioHandler>();
        services.AddScoped<FazerLoginHandler>();
        services.AddScoped<CadastrarIncubatorioHandler>();
        services.AddScoped<CadastrarLoteHandler>();
        services.AddScoped<ReceberOvoHandler>();
        services.AddScoped<ConsultarOvoHandler>();

        return services;
    }
}
