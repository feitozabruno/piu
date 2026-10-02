using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Piu.Api.Features.Auth;

public class OperadorConfiguration : IEntityTypeConfiguration<Operador>
{
    public void Configure(EntityTypeBuilder<Operador> builder)
    {
        builder.Property(o => o.Nome).IsRequired().HasMaxLength(200);
        builder.Property(o => o.Cargo).IsRequired().HasMaxLength(50);
        builder.Property(o => o.Ativo).IsRequired().HasDefaultValue(true);
    }
}
