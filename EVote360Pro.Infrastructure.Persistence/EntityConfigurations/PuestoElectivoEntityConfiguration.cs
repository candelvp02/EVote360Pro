using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class PuestoElectivoEntityConfiguration : IEntityTypeConfiguration<PuestoElectivo>
    {
        public void Configure(EntityTypeBuilder<PuestoElectivo> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("PuestosElectivos");
            #endregion

            #region Property configurations
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Descripcion).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Activo).IsRequired();
            #endregion

            #region Relationships
            builder.HasMany<CandidatoPuesto>(p => p.CandidatoPuestos)
                .WithOne(cp => cp.PuestoElectivo)
                .HasForeignKey(cp => cp.PuestoElectivoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany<Voto>(p => p.Votos)
                .WithOne(v => v.PuestoElectivo)
                .HasForeignKey(v => v.PuestoElectivoId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}