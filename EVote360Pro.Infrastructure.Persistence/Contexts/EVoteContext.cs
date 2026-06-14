using EVote360Pro.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EVote360Pro.Infrastructure.Persistence.Contexts
{
    public class EVoteContext : DbContext
    {
        public EVoteContext(DbContextOptions<EVoteContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ciudadano> Ciudadanos { get; set; }
        public DbSet<PuestoElectivo> PuestosElectivos { get; set; }
        public DbSet<PartidoPolitico> PartidosPoliticos { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<DirigentePolitico> DirigentesPoliticos { get; set; }
        public DbSet<Eleccion> Elecciones { get; set; }
        public DbSet<CodigoVerificacion> CodigosVerificacion { get; set; }
        public DbSet<Voto> Votos { get; set; }
        public DbSet<SolicitudAlianza> SolicitudesAlianza { get; set; }
        public DbSet<CandidatoPuesto> CandidatosPuestos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}