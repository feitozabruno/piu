using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Piu.Api.Features.Auth;
using Piu.Api.Features.Incubatorios;

namespace Piu.Api.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.Property(u => u.Nome).IsRequired().HasMaxLength(UsuarioLimites.NomeMax);
        builder.Property(u => u.Cargo).IsRequired().HasMaxLength(UsuarioLimites.CargoMax);
        builder.Property(u => u.Ativo).IsRequired().HasDefaultValue(true);
        builder.Property(u => u.Perfil).IsRequired().HasMaxLength(30).HasDefaultValue(Usuario.Funcionario);

        builder.Property(u => u.Incubatorio)
               .HasColumnType("citext")
               .IsRequired(false);

        builder.HasOne<Incubatorio>()
               .WithMany()
               .HasForeignKey(u => u.Incubatorio)
               .HasPrincipalKey(i => i.Identificador)
               .OnDelete(DeleteBehavior.Restrict)
               .HasConstraintName(UsuarioConstraints.FkIncubatorio);
    }
}
