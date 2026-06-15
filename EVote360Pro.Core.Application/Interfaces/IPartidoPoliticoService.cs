using EVote360Pro.Core.Application.Dtos.PartidoPolitico;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface IPartidoPoliticoService
    {
        Task<PartidoPoliticoDto?> AddAsync(SavePartidoPoliticoDto dto);
        Task<PartidoPoliticoDto?> UpdateAsync(SavePartidoPoliticoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<PartidoPoliticoDto>> GetAll();
        Task<List<PartidoPoliticoDto>> GetAllActivos();
        Task<PartidoPoliticoDto?> GetById(int id);
        Task<bool> CambiarEstadoAsync(int id);
        Task<bool> ParticipoenEleccionAsync(int id);
    }
}