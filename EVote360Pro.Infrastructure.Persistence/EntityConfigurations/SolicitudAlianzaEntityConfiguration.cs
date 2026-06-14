using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class SolicitudAlianzaEntityConfiguration : IEntityTypeConfiguration<SolicitudAlianza>
    {
        public void Configure(EntityTypeBuilder<SolicitudAlianza> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("SolicitudesAlianza");
            #endregion

            #region Property configurations
            builder.Property(s => s.FechaSolicitud).IsRequired();
            builder.Property(s => s.FechaRespuesta).IsRequired(false);
            builder.Property(s => s.Estado).IsRequired();
            builder.Property(s => s.Activo).IsRequired();
            #endregion
        }
    }
}