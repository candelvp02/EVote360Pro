using EVote360Pro.Core.Application.Dtos.DirigentePolitico;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface IDirigentePoliticoService
    {
        Task<DirigentePoliticoDto?> AddAsync(SaveDirigentePoliticoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<DirigentePoliticoDto>> GetAll();
        Task<List<DirigentePoliticoDto>> GetAllWithInclude();
        Task<DirigentePoliticoDto?> GetById(int id);
        Task<DirigentePoliticoDto?> GetByUsuarioId(int usuarioId);
    }
}