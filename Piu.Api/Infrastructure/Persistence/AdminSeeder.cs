using Microsoft.AspNetCore.Identity;
using Piu.Api.Features.Auth;

namespace Piu.Api.Infrastructure.Persistence;

public static class AdminSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        var username = configuration["Admin:Usuario"];
        var password = configuration["Admin:Senha"];
        var name = configuration["Admin:Nome"] ?? "Administrador do sistema";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return;

        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        if (await userManager.FindByNameAsync(username) is not null)
            return;

        var admin = Usuario.Criar(username, name, "Administrador", "ADMIN", Usuario.Administrador);
        var result = await userManager.CreateAsync(admin, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(error => error.Code));
            throw new InvalidOperationException($"Não foi possível criar o administrador inicial: {errors}");
        }
    }
}
