using EVote360Pro.Core.Application.Dtos.Eleccion;
using EVote360Pro.Core.Application.ViewModels.Eleccion;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface IEleccionService
    {
        Task<EleccionDto?> AddAsync(SaveEleccionDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<EleccionDto>> GetAll();
        Task<EleccionDto?> GetById(int id);
        Task<EleccionDto?> GetEleccionActiva();
        Task<bool> ActivarEleccionAsync(int id);
        Task<bool> FinalizarEleccionAsync(int id);
        Task<ResultadosEleccionViewModel?> GetResultados(int eleccionId);
        Task<List<int>> GetAnosConElecciones();
        Task<List<EleccionDto>> GetByAno(int ano);
    }
}