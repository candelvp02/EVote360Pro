using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class EleccionRepository : GenericRepository<Eleccion>, IEleccionRepository
    {
        private readonly EVoteContext _context;

        public EleccionRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Eleccion?> GetEleccionActiva()
        {
            return await _context.Elecciones
                .FirstOrDefaultAsync(e => e.Estado == EstadoEleccion.Activa);
        }

        public async Task<bool> ExisteEleccionActiva()
        {
            return await _context.Elecciones
                .AnyAsync(e => e.Estado == EstadoEleccion.Activa);
        }

        public async Task<List<int>> GetAnosConElecciones()
        {
            return await _context.Elecciones
                .Select(e => e.FechaRealizacion.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();
        }

        public async Task<List<Eleccion>> GetByAno(int ano)
        {
            return await _context.Elecciones
                .Where(e => e.FechaRealizacion.Year == ano)
                .OrderByDescending(e => e.FechaRealizacion)
                .ToListAsync();
        }
    }
}