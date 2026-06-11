using EVote360Pro.Core.Application.Dtos.Ciudadano;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface ICiudadanoService
    {
        Task<CiudadanoDto?> AddAsync(SaveCiudadanoDto dto);
        Task<CiudadanoDto?> UpdateAsync(SaveCiudadanoDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<CiudadanoDto>> GetAll();
        Task<CiudadanoDto?> GetById(int id);
        Task<CiudadanoDto?> GetByNumeroDocumento(string numeroDocumento);
        Task<bool> CambiarEstadoAsync(int id);
    }
}