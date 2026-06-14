using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface IPuestoElectivoRepository : IGenericRepository<PuestoElectivo>
    {
        Task<bool> ExisteNombre(string nombre, int? excludeId = null);
        Task<bool> TieneCandidatosAsignados(int puestoId);
        Task<bool> ParticipoenEleccion(int puestoId);
        Task<List<PuestoElectivo>> GetAllActivos();
    }
}