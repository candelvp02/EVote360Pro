using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface ICandidatoPuestoRepository : IGenericRepository<CandidatoPuesto>
    {
        Task<List<CandidatoPuesto>> GetByPartido(int partidoId);
        Task<CandidatoPuesto?> GetByCandidatoYPartido(int candidatoId, int partidoId);
        Task<CandidatoPuesto?> GetByPuestoYPartido(int puestoId, int partidoId);
        Task<bool> ExisteCandidatoAliado(int candidatoId, int partidoSolicitanteId, int candidatoPuestoPartidOrigenId);
    }
}