using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface ICiudadanoRepository : IGenericRepository<Ciudadano>
    {
        Task<Ciudadano?> GetByNumeroDocumento(string numeroDocumento);
        Task<bool> ExisteNumeroDocumento(string numeroDocumento, int? excludeId = null);
        Task<bool> ExisteCorreo(string correo, int? excludeId = null);
        Task<bool> YaVotoEnEleccion(int ciudadanoId, int eleccionId);
        Task<bool> ParticipoEnEleccion(int ciudadanoId);
    }
}