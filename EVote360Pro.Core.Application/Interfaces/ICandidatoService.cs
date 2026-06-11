using EVote360Pro.Core.Application.Dtos.Candidato;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface ICandidatoService
    {
        Task<CandidatoDto?> AddAsync(SaveCandidatoDto dto);
        Task<CandidatoDto?> UpdateAsync(SaveCandidatoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<CandidatoDto>> GetAll();
        Task<List<CandidatoDto>> GetByPartido(int partidoId);
        Task<CandidatoDto?> GetById(int id);
        Task<bool> CambiarEstadoAsync(int id);
    }
}