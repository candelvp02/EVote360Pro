using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class CandidatoRepository : GenericRepository<Candidato>, ICandidatoRepository
    {
        private readonly EVoteContext _context;

        public CandidatoRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Candidato>> GetByPartido(int partidoId)
        {
            return await _context.Candidatos
                .Where(c => c.PartidoPoliticoId == partidoId)
                .ToListAsync();
        }

        public async Task<bool> TienePuestoAsignado(int candidatoId)
        {
            return await _context.CandidatosPuestos
                .AnyAsync(cp => cp.CandidatoId == candidatoId);
        }

        public async Task<bool> ParticipoenEleccion(int candidatoId)
        {
            return await _context.Votos
                .AnyAsync(v => v.CandidatoId == candidatoId);
        }
    }
}