using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class CandidatoPuestoEntityConfiguration : IEntityTypeConfiguration<CandidatoPuesto>
    {
        public void Configure(EntityTypeBuilder<CandidatoPuesto> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("CandidatosPuestos");
            #endregion

            #region Property configurations
            builder.Property(cp => cp.CandidatoId).IsRequired();
            builder.Property(cp => cp.PuestoElectivoId).IsRequired();
            builder.Property(cp => cp.PartidoPoliticoId).IsRequired();
            builder.Property(cp => cp.Activo).IsRequired();
            #endregion
        }
    }
}