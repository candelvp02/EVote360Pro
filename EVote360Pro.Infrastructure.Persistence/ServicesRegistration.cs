using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using EVote360Pro.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EVote360Pro.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EVoteContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(EVoteContext).Assembly.FullName)));

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<ICiudadanoRepository, CiudadanoRepository>();
            services.AddTransient<IPuestoElectivoRepository, PuestoElectivoRepository>();
            services.AddTransient<IPartidoPoliticoRepository, PartidoPoliticoRepository>();
            services.AddTransient<ICandidatoRepository, CandidatoRepository>();
            services.AddTransient<IDirigentePoliticoRepository, DirigentePoliticoRepository>();
            services.AddTransient<IEleccionRepository, EleccionRepository>();
            services.AddTransient<ICodigoVerificacionRepository, CodigoVerificacionRepository>();
            services.AddTransient<IVotoRepository, VotoRepository>();
            services.AddTransient<ISolicitudAlianzaRepository, SolicitudAlianzaRepository>();
            services.AddTransient<ICandidatoPuestoRepository, CandidatoPuestoRepository>();
            #endregion
        }
    }
}