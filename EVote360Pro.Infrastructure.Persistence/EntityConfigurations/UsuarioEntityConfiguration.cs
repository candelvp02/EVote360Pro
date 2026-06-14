using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EVote360Pro.Infrastructure.Persistence.EntityConfigurations
{
    public class UsuarioEntityConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Usuarios");
            #endregion

            #region Property configurations
            builder.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(u => u.CorreoElectronico).IsRequired().HasMaxLength(255);
            builder.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Contrasena).IsRequired().HasMaxLength(500);
            builder.Property(u => u.Rol).IsRequired();
            builder.Property(u => u.Activo).IsRequired();
            #endregion

            #region Relationships
            builder.HasOne<DirigentePolitico>(u => u.DirigentePolitico)
                .WithOne(d => d.Usuario)
                .HasForeignKey<DirigentePolitico>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}