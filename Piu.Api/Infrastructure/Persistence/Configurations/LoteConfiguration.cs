using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Piu.Api.Features.Lotes;

namespace Piu.Api.Infrastructure.Persistence.Configurations;

public class LoteConfiguration : IEntityTypeConfiguration<Lote>
{
    public void Configure(EntityTypeBuilder<Lote> builder)
    {
        builder.ToTable("Lotes");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Nome).IsRequired().HasMaxLength(LoteLimites.NomeMax);
        builder.HasIndex(l => l.Nome).IsUnique().HasDatabaseName("IX_Lotes_Nome");
        builder.Property(l => l.DataNascimento).IsRequired();
    }
}
