using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class PartidoPoliticoEntityConfiguration : IEntityTypeConfiguration<PartidoPolitico>
    {
        public void Configure(EntityTypeBuilder<PartidoPolitico> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("PartidosPoliticos");
            #endregion

            #region Property configurations
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            builder.Property(p => p.Descripcion).HasMaxLength(500);
            builder.Property(p => p.Siglas).IsRequired().HasMaxLength(20);
            builder.Property(p => p.LogoUrl).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Activo).IsRequired();
            #endregion

            #region Relationships
            builder.HasMany<Candidato>(p => p.Candidatos)
                .WithOne(c => c.PartidoPolitico)
                .HasForeignKey(c => c.PartidoPoliticoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<DirigentePolitico>(p => p.DirigentePolitico)
                .WithOne(d => d.PartidoPolitico)
                .HasForeignKey<DirigentePolitico>(d => d.PartidoPoliticoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany<SolicitudAlianza>(p => p.SolicitudesEnviadas)
                .WithOne(s => s.PartidoSolicitante)
                .HasForeignKey(s => s.PartidoSolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany<SolicitudAlianza>(p => p.SolicitudesRecibidas)
                .WithOne(s => s.PartidoReceptor)
                .HasForeignKey(s => s.PartidoReceptorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany<CandidatoPuesto>(p => p.CandidatosPuestos)
                .WithOne(cp => cp.PartidoPolitico)
                .HasForeignKey(cp => cp.PartidoPoliticoId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}