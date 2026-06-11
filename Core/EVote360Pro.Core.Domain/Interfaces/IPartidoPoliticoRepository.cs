using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface IPartidoPoliticoRepository : IGenericRepository<PartidoPolitico>
    {
        Task<bool> ExisteSiglas(string siglas, int? excludeId = null);
        Task<bool> TieneCandidatosActivos(int partidoId);
        Task<bool> TieneDirigenteAsignado(int partidoId);
        Task<bool> ParticipoEnEleccion(int partidoId);
        Task<List<PartidoPolitico>> GetAllActivos();
    }
}