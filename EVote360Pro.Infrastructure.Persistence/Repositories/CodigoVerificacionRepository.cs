using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class CodigoVerificacionRepository : GenericRepository<CodigoVerificacion>, ICodigoVerificacionRepository
    {
        private readonly EVoteContext _context;

        public CodigoVerificacionRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CodigoVerificacion?> GetUltimoVigente(int ciudadanoId, int eleccionId)
        {
            return await _context.CodigosVerificacion
                .Where(c => c.CiudadanoId == ciudadanoId
                    && c.EleccionId == eleccionId
                    && !c.Usado
                    && c.FechaExpiracion > DateTime.Now)
                .OrderByDescending(c => c.FechaGeneracion)
                .FirstOrDefaultAsync();
        }
    }
}