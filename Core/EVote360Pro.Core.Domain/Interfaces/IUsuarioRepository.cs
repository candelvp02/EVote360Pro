using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario?> GetByNombreUsuario(string nombreUsuario);
        Task<bool> ExisteNombreUsuario(string nombreUsuario, int? excludeId = null);
        Task<bool> ExisteCorreo(string correo, int? excludeId = null);
        Task<int> ContarAdministradoresActivos();
    }
}