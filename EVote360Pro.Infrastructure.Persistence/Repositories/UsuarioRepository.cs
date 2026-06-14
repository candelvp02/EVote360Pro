using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly EVoteContext _context;

        public UsuarioRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByNombreUsuario(string nombreUsuario)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }

        public async Task<bool> ExisteNombreUsuario(string nombreUsuario, int? excludeId = null)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.NombreUsuario == nombreUsuario && (excludeId == null || u.Id != excludeId));
        }

        public async Task<bool> ExisteCorreo(string correo, int? excludeId = null)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.CorreoElectronico == correo && (excludeId == null || u.Id != excludeId));
        }

        public async Task<int> ContarAdministradoresActivos()
        {
            return await _context.Usuarios
                .CountAsync(u => u.Rol == RolUsuario.Administrador && u.Activo);
        }
    }
}