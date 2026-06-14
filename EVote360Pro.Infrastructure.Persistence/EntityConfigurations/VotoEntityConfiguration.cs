using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class VotoEntityConfiguration : IEntityTypeConfiguration<Voto>
    {
        public void Configure(EntityTypeBuilder<Voto> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Votos");
            #endregion

            #region Property configurations
            builder.Property(v => v.CiudadanoId).IsRequired();
            builder.Property(v => v.EleccionId).IsRequired();
            builder.Property(v => v.PuestoElectivoId).IsRequired();
            builder.Property(v => v.CandidatoId).IsRequired(false);
            builder.Property(v => v.EsNinguno).IsRequired();
            builder.Property(v => v.Finalizado).IsRequired();
            builder.Property(v => v.Activo).IsRequired();
            #endregion
        }
    }
}