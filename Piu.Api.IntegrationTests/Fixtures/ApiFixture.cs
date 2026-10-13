using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Piu.Api.Features.Incubatorios;
using Piu.Api.Infrastructure.Persistence;
using Respawn;
using Testcontainers.PostgreSql;

namespace Piu.Api.IntegrationTests.Fixtures;

public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres =
        new PostgreSqlBuilder("postgres:18.6-alpine3.24")
            .WithDatabase("piu_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    private Respawner _respawner = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(_postgres.GetConnectionString());
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            await db.Database.MigrateAsync();
        }

        await using var connection =
            new NpgsqlConnection(_postgres.GetConnectionString());

        await connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
    }

    public async Task ResetDatabaseAsync()
    {
        await using var connection =
            new NpgsqlConnection(_postgres.GetConnectionString());

        await connection.OpenAsync();

        await _respawner.ResetAsync(connection);

        await using var scope = Services.CreateAsyncScope();

        var db = scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

        db.Incubatorios.Add(Incubatorio.Criar(
            identificador: "INC-001",
            nome: "Incubatorio de Teste",
            sigla: "ICT"));

        await db.SaveChangesAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await base.DisposeAsync();
        await _postgres.DisposeAsync();
    }
}

[CollectionDefinition("API")]
public sealed class ApiCollection
    : ICollectionFixture<CustomWebApplicationFactory>
{
}
