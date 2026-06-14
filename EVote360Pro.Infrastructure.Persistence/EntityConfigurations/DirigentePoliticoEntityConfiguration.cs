using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class DirigentePoliticoEntityConfiguration : IEntityTypeConfiguration<DirigentePolitico>
    {
        public void Configure(EntityTypeBuilder<DirigentePolitico> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("DirigentesPoliticos");
            #endregion

            #region Property configurations
            builder.Property(d => d.UsuarioId).IsRequired();
            builder.Property(d => d.PartidoPoliticoId).IsRequired();
            builder.Property(d => d.Activo).IsRequired();
            #endregion
        }
    }
}