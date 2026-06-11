using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Enums;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface ISolicitudAlianzaRepository : IGenericRepository<SolicitudAlianza>
    {
        Task<bool> ExisteAlianzaVigente(int partidoId, int partido2Id);
        Task<bool> ExisteSolicitudPendiente(int partidoId, int partido2Id);
        Task<List<SolicitudAlianza>> GetSolicitudesPendientesParaPartido(int partidoId);
        Task<List<SolicitudAlianza>> GetAlianzasVigentes(int partidoId);
    }
}