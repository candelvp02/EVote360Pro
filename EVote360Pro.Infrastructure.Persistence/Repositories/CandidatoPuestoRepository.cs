using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class CandidatoPuestoRepository : GenericRepository<CandidatoPuesto>, ICandidatoPuestoRepository
    {
        private readonly EVoteContext _context;

        public CandidatoPuestoRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CandidatoPuesto>> GetByPartido(int partidoId)
        {
            return await _context.CandidatosPuestos
                .Include(cp => cp.Candidato)
                .Include(cp => cp.PuestoElectivo)
                .Include(cp => cp.PartidoPolitico)
                .Where(cp => cp.PartidoPoliticoId == partidoId)
                .ToListAsync();
        }

        public async Task<CandidatoPuesto?> GetByCandidatoYPartido(int candidatoId, int partidoId)
        {
            return await _context.CandidatosPuestos
                .FirstOrDefaultAsync(cp => cp.CandidatoId == candidatoId
                    && cp.PartidoPoliticoId == partidoId);
        }

        public async Task<CandidatoPuesto?> GetByPuestoYPartido(int puestoId, int partidoId)
        {
            return await _context.CandidatosPuestos
                .FirstOrDefaultAsync(cp => cp.PuestoElectivoId == puestoId
                    && cp.PartidoPoliticoId == partidoId);
        }

        public async Task<bool> ExisteCandidatoAliado(int candidatoId, int partidoSolicitanteId, int candidatoPuestoPartidoOrigenId)
        {
            return await _context.CandidatosPuestos
                .AnyAsync(cp => cp.CandidatoId == candidatoId
                    && cp.PartidoPoliticoId == partidoSolicitanteId);
        }
    }
}