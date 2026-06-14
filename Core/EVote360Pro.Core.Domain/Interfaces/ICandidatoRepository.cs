using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface ICandidatoRepository : IGenericRepository<Candidato>
    {
        Task<List<Candidato>> GetByPartido(int partidoId);
        Task<bool> TienePuestoAsignado(int candidatoId);
        Task<bool> ParticipoenEleccion(int candidatoId);
    }
}