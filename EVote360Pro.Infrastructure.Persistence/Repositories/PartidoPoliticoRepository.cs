using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class PartidoPoliticoRepository : GenericRepository<PartidoPolitico>, IPartidoPoliticoRepository
    {
        private readonly EVoteContext _context;

        public PartidoPoliticoRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteSiglas(string siglas, int? excludeId = null)
        {
            return await _context.PartidosPoliticos
                .AnyAsync(p => p.Siglas == siglas && (excludeId == null || p.Id != excludeId));
        }

        public async Task<bool> TieneCandidatosActivos(int partidoId)
        {
            return await _context.Candidatos
                .AnyAsync(c => c.PartidoPoliticoId == partidoId && c.Activo);
        }

        public async Task<bool> TieneDirigenteAsignado(int partidoId)
        {
            return await _context.DirigentesPoliticos
                .AnyAsync(d => d.PartidoPoliticoId == partidoId);
        }

        public async Task<bool> ParticipoenEleccion(int partidoId)
        {
            return await _context.Candidatos
                .Where(c => c.PartidoPoliticoId == partidoId)
                .AnyAsync(c => c.Votos != null && c.Votos.Any());
        }

        public async Task<List<PartidoPolitico>> GetAllActivos()
        {
            return await _context.PartidosPoliticos
                .Where(p => p.Activo)
                .ToListAsync();
        }
    }
}