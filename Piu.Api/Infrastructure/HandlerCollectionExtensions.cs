using Piu.Api.Features.Auth.CadastrarUsuario;
using Piu.Api.Features.Auth.FazerLogin;
using Piu.Api.Features.Incubatorios.CadastrarIncubatorio;
using Piu.Api.Features.Lotes.CadastrarLote;

namespace Piu.Api.Infrastructure;

public static class HandlerCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<CadastrarUsuarioHandler>();
        services.AddScoped<FazerLoginHandler>();
        services.AddScoped<CadastrarIncubatorioHandler>();
        services.AddScoped<CadastrarLoteHandler>();

        return services;
    }
}
