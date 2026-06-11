using EVote360Pro.Core.Application.Services;
using EVote360Pro.Core.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EVote360Pro.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayer( this IServiceCollection services)
        {
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<ICiudadanoService, CiudadanoService>();
            services.AddTransient<IPuestoElectivoService, PuestoElectivoService>();
            services.AddTransient<IPartidoPoliticoService, PartidoPoliticoService>();
            services.AddTransient<ICandidatoService, CandidatoService>();
            services.AddTransient<IDirigentePoliticoService, DirigentePoliticoService>();
            services.AddTransient<IEleccionService, EleccionService>();
            services.AddTransient<IVotacionService, VotacionService>();
            services.AddTransient<ISolicitudAlianzaService, SolicitudAlianzaService>();
            services.AddTransient<ICandidatoPuestoService, CandidatoPuestoService>();

        }
    }
}