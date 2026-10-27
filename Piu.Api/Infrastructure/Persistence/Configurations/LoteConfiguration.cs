using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Piu.Api.Features.Lotes;

namespace Piu.Api.Infrastructure.Persistence.Configurations;

public class LoteConfiguration : IEntityTypeConfiguration<Lote>
{
    public void Configure(EntityTypeBuilder<Lote> builder)
    {
        builder.ToTable("Lotes");

        builder.Property(lote => lote.IncubatorioIdentificador)
            .HasColumnType("citext")
            .IsRequired();

        builder.HasOne(lote => lote.Incubatorio)
               .WithMany()
               .HasForeignKey(lote => lote.IncubatorioIdentificador)
               .HasPrincipalKey(incubatorio => incubatorio.Identificador)
             .OnDelete(DeleteBehavior.Restrict)
             .IsRequired()
             .HasConstraintName("FK_Lotes_Incubatorios_IncubatorioIdentificador");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Nome).IsRequired().HasMaxLength(LoteLimites.NomeMax);
        builder.HasIndex(l => new { l.IncubatorioIdentificador, l.Nome })
            .IsUnique()
            .HasDatabaseName("IX_Lotes_IncubatorioIdentificador_Nome");
        builder.Property(l => l.DataNascimento).IsRequired();
        builder.Property(l => l.Ativo)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
