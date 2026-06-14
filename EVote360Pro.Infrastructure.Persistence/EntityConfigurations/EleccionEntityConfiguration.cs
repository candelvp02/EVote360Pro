using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class EleccionEntityConfiguration : IEntityTypeConfiguration<Eleccion>
    {
        public void Configure(EntityTypeBuilder<Eleccion> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Elecciones");
            #endregion

            #region Property configurations
            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            builder.Property(e => e.FechaRealizacion).IsRequired();
            builder.Property(e => e.Estado).IsRequired();
            builder.Property(e => e.Activo).IsRequired();
            #endregion

            #region Relationships
            builder.HasMany<Voto>(e => e.Votos)
                .WithOne(v => v.Eleccion)
                .HasForeignKey(v => v.EleccionId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}