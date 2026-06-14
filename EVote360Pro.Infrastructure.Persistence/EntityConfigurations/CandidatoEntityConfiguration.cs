using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class CandidatoEntityConfiguration : IEntityTypeConfiguration<Candidato>
    {
        public void Configure(EntityTypeBuilder<Candidato> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Candidatos");
            #endregion

            #region Property configurations
            builder.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(c => c.FotoUrl).IsRequired().HasMaxLength(500);
            builder.Property(c => c.Activo).IsRequired();
            #endregion

            #region Relationships
            builder.HasMany<CandidatoPuesto>(c => c.CandidatoPuestos)
                .WithOne(cp => cp.Candidato)
                .HasForeignKey(cp => cp.CandidatoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany<Voto>(c => c.Votos)
                .WithOne(v => v.Candidato)
                .HasForeignKey(v => v.CandidatoId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}