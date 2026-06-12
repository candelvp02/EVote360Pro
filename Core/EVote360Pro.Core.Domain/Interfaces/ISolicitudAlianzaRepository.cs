using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface ISolicitudAlianzaRepository : IGenericRepository<SolicitudAlianza>
    {
        Task<bool> ExisteAlianzaVigente(int partido1Id, int partido2Id);
        Task<bool> ExisteSolicitudPendiente(int partido1Id, int partido2Id);
        Task<List<SolicitudAlianza>> GetSolicitudesPendientesParaPartido(int partidoId);
        Task<List<SolicitudAlianza>> GetSolicitudesRealizadasPorPartido(int partidoId);
        Task<List<SolicitudAlianza>> GetAlianzasVigentes(int partidoId);
    }
}