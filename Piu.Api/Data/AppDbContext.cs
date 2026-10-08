using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Piu.Api.Features.Auth;

namespace Piu.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<Usuario>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UsuarioConfiguration());
    }
}
