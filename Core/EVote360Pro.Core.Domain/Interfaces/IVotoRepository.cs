using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface IVotoRepository :IGenericRepository<Voto>
    {
        Task<List<Voto>> GetByEleccionYCiudadano(int eleccionId, int ciudadanoID);
        Task<List<Voto>> GetByEleccion(int eleccionId);
        Task<bool> CiudadanoFinalizoVoto(int ciudadanoId, int eleccionId);
    }
}