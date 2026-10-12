using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Piu.Api.Features.Incubatorios;

namespace Piu.Api.Infrastructure.Persistence.Configurations;

public class IncubatorioConfiguration : IEntityTypeConfiguration<Incubatorio>
{
    public void Configure(EntityTypeBuilder<Incubatorio> builder)
    {
        builder.ToTable("Incubatorios", t =>
            t.HasCheckConstraint(
                IncubatorioConstraints.CkIdentificadorTamanho,
                $"char_length(\"Identificador\") BETWEEN {IncubatorioLimites.IdentificadorMin} AND {IncubatorioLimites.IdentificadorMax}"));

        builder.HasKey(i => i.Identificador)
               .HasName(IncubatorioConstraints.PkIdentificador);

        builder.Property(i => i.Identificador)
               .HasColumnType("citext")
               .ValueGeneratedNever()
               .IsRequired();

        builder.Property(i => i.Nome)
               .HasMaxLength(IncubatorioLimites.NomeMax)
               .IsRequired();

        builder.Property(i => i.Sigla)
               .HasMaxLength(IncubatorioLimites.SiglaMax)
               .IsRequired();

        builder.Property(i => i.Ativo)
               .IsRequired();

        builder.Property(i => i.DataCadastro)
               .IsRequired();
    }
}
