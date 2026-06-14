using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class CodigoVerificacionEntityConfiguration : IEntityTypeConfiguration<CodigoVerificacion>
    {
        public void Configure(EntityTypeBuilder<CodigoVerificacion> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("CodigosVerificacion");
            #endregion

            #region Property configurations
            builder.Property(c => c.Codigo).IsRequired().HasMaxLength(10);
            builder.Property(c => c.FechaGeneracion).IsRequired();
            builder.Property(c => c.FechaExpiracion).IsRequired();
            builder.Property(c => c.Usado).IsRequired();
            builder.Property(c => c.Activo).IsRequired();
            #endregion
        }
    }
}