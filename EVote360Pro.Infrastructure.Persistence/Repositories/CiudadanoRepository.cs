using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class CiudadanoRepository : GenericRepository<Ciudadano>, ICiudadanoRepository
    {
        private readonly EVoteContext _context;

        public CiudadanoRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Ciudadano?> GetByNumeroDocumento(string numeroDocumento)
        {
            return await _context.Ciudadanos
                .FirstOrDefaultAsync(c => c.NumeroDocumento == numeroDocumento);
        }

        public async Task<bool> ExisteNumeroDocumento(string numeroDocumento, int? excludeId = null)
        {
            return await _context.Ciudadanos
                .AnyAsync(c => c.NumeroDocumento == numeroDocumento && (excludeId == null || c.Id != excludeId));
        }

        public async Task<bool> ExisteCorreo(string correo, int? excludeId = null)
        {
            return await _context.Ciudadanos
                .AnyAsync(c => c.CorreoElectronico == correo && (excludeId == null || c.Id != excludeId));
        }

        public async Task<bool> YaVotoEnEleccion(int ciudadanoId, int eleccionId)
        {
            return await _context.Votos
                .AnyAsync(v => v.CiudadanoId == ciudadanoId && v.EleccionId == eleccionId && v.Finalizado);
        }

        public async Task<bool> ParticipoenEleccion(int ciudadanoId)
        {
            return await _context.Votos
                .AnyAsync(v => v.CiudadanoId == ciudadanoId);
        }
    }
}