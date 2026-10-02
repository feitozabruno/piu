using Microsoft.AspNetCore.Identity;
using Piu.Api.Data;
using Piu.Api.Features.Auth.CadastrarOperador;

namespace Piu.Api.Features.Auth;

public static class AuthExtension
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(IdentityConstants.BearerScheme)
                .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services.AddIdentityCore<Operador>(options =>
        {
            options.User.RequireUniqueEmail = false;

            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<AppDbContext>();

        services.AddScoped<CadastrarOperadorService>();

        return services;
    }
}
