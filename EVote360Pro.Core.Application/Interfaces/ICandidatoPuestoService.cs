using EVote360Pro.Core.Application.Dtos.CandidatoPuesto;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface ICandidatoPuestoService
    {
        Task<CandidatoPuestoDto?> AddAsync(SaveCandidatoPuestoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<CandidatoPuestoDto>> GetByPartido(int partidoId);
        Task<CandidatoPuestoDto?> GetById(int id);
    }
}