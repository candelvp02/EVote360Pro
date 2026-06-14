using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class PuestoElectivoRepository : GenericRepository<PuestoElectivo>, IPuestoElectivoRepository
    {
        private readonly EVoteContext _context;

        public PuestoElectivoRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteNombre(string nombre, int? excludeId = null)
        {
            return await _context.PuestosElectivos
                .AnyAsync(p => p.Nombre == nombre && (excludeId == null || p.Id != excludeId));
        }

        public async Task<bool> TieneCandidatosAsignados(int puestoId)
        {
            return await _context.CandidatosPuestos
                .AnyAsync(cp => cp.PuestoElectivoId == puestoId);
        }

        public async Task<bool> ParticipoenEleccion(int puestoId)
        {
            return await _context.Votos
                .AnyAsync(v => v.PuestoElectivoId == puestoId);
        }

        public async Task<List<PuestoElectivo>> GetAllActivos()
        {
            return await _context.PuestosElectivos
                .Where(p => p.Activo)
                .ToListAsync();
        }
    }
}