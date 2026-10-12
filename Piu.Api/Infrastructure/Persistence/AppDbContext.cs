using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Piu.Api.Features.Auth;
using Piu.Api.Features.Incubatorios;
using Piu.Api.Infrastructure.Persistence.Configurations;

namespace Piu.Api.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<Usuario>(options)
{
    public DbSet<Incubatorio> Incubatorios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("citext");

        builder.ApplyConfiguration(new UsuarioConfiguration());
        builder.ApplyConfiguration(new IncubatorioConfiguration());
    }
}
