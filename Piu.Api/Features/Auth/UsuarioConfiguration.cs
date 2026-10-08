using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Piu.Api.Features.Auth;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.Property(u => u.Nome).IsRequired().HasMaxLength(200);
        builder.Property(u => u.Cargo).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Incubatorio).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Ativo).IsRequired().HasDefaultValue(true);
    }
}
