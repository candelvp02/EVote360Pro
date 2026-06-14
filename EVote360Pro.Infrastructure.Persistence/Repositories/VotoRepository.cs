using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class VotoRepository : GenericRepository<Voto>, IVotoRepository
    {
        private readonly EVoteContext _context;

        public VotoRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Voto>> GetByEleccionYCiudadano(int eleccionId, int ciudadanoId)
        {
            return await _context.Votos
                .Where(v => v.EleccionId == eleccionId && v.CiudadanoId == ciudadanoId)
                .ToListAsync();
        }

        public async Task<List<Voto>> GetByEleccion(int eleccionId)
        {
            return await _context.Votos
                .Include(v => v.Candidato)
                .ThenInclude(c => c.PartidoPolitico)
                .Where(v => v.EleccionId == eleccionId && v.Finalizado)
                .ToListAsync();
        }

        public async Task<bool> CiudadanoFinalizoVoto(int ciudadanoId, int eleccionId)
        {
            return await _context.Votos
                .AnyAsync(v => v.CiudadanoId == ciudadanoId
                    && v.EleccionId == eleccionId
                    && v.Finalizado);
        }
    }
}