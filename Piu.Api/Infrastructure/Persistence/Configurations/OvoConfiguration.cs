using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Piu.Api.Features.Ovos;

namespace Piu.Api.Infrastructure.Persistence.Configurations;

public sealed class OvoConfiguration : IEntityTypeConfiguration<Ovo>
{
    public void Configure(EntityTypeBuilder<Ovo> builder)
    {
        builder.ToTable("Ovos");

        builder.HasKey(ovo => ovo.Id);

        builder.Property(ovo => ovo.DataPostura).IsRequired();
        builder.Property(ovo => ovo.NinhoIncubavel).IsRequired();
        builder.Property(ovo => ovo.CamaIncubavel).IsRequired();
        builder.Property(ovo => ovo.Idade).IsRequired().HasMaxLength(20);

        builder.Property(ovo => ovo.TemperaturaRecebimento)
               .IsRequired()
              .HasPrecision(10, 2);

        builder.Property(ovo => ovo.TemperaturaBau)
               .IsRequired()
               .HasPrecision(10, 2);

        builder.Property(ovo => ovo.PlacaCaminhao).IsRequired();
        builder.Property(ovo => ovo.Motorista).IsRequired();
        builder.Property(ovo => ovo.HoraRecebimento).IsRequired();
        builder.Property(ovo => ovo.Observacao).IsRequired();

        builder.HasOne(ovo => ovo.Lote)
               .WithMany()
               .HasForeignKey(ovo => ovo.LoteId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired()
               .HasConstraintName("FK_Ovos_Lotes_LoteId");
    }
}
