using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class CiudadanoEntityConfiguration : IEntityTypeConfiguration<Ciudadano>
    {
        public void Configure(EntityTypeBuilder<Ciudadano> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Ciudadanos");
            #endregion

            #region Property configurations
            builder.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(c => c.CorreoElectronico).IsRequired().HasMaxLength(255);
            builder.Property(c => c.NumeroDocumento).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Activo).IsRequired();
            #endregion

            #region Relationships
            builder.HasMany<CodigoVerificacion>(c => c.CodigoVerificacion)
                .WithOne(cv => cv.Ciudadano)
                .HasForeignKey(cv => cv.CiudadanoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany<Voto>(c => c.Votos)
                .WithOne(v => v.Ciudadano)
                .HasForeignKey(v => v.CiudadanoId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}