using EVote360Pro.Core.Application.Dtos.Usuario;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> AddAsync(SaveUsuarioDto dto);
        Task<UsuarioDto?> UpdateAsync(SaveUsuarioDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<UsuarioDto>> GetAll();
        Task<List<UsuarioDto>> GetAllWithInclude();
        Task<UsuarioDto?> GetById(int id);
        Task<UsuarioDto?> LoginAsync(string nombreUsuario, string contrasena);
        Task<bool> CambiarEstadoAsync(int id);
    }
}