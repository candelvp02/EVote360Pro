using EVote360Pro.Core.Application.Dtos.PuestoElectivo;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface IPuestoElectivoService
    {
        Task<PuestoElectivoDto?> AddAsync(SavePuestoElectivoDto dto);
        Task<PuestoElectivoDto?> UpdateAsync(SavePuestoElectivoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<PuestoElectivoDto>> GetAll();
        Task<List<PuestoElectivoDto>> GetAllActivos();
        Task<PuestoElectivoDto?> GetById(int id);
        Task<bool> CambiarEstadoAsync(int id);
        Task<bool> ParticipoenEleccionAsync(int id);
    }
}