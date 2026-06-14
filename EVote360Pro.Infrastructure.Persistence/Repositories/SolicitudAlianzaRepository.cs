using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class SolicitudAlianzaRepository : GenericRepository<SolicitudAlianza>, ISolicitudAlianzaRepository
    {
        private readonly EVoteContext _context;

        public SolicitudAlianzaRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteAlianzaVigente(int partido1Id, int partido2Id)
        {
            return await _context.SolicitudesAlianza
                .AnyAsync(s => s.Estado == EstadoSolicitudAlianza.Aceptada
                    && ((s.PartidoSolicitanteId == partido1Id && s.PartidoReceptorId == partido2Id)
                    || (s.PartidoSolicitanteId == partido2Id && s.PartidoReceptorId == partido1Id)));
        }

        public async Task<bool> ExisteSolicitudPendiente(int partido1Id, int partido2Id)
        {
            return await _context.SolicitudesAlianza
                .AnyAsync(s => s.Estado == EstadoSolicitudAlianza.EnEsperaDeRespuesta
                    && ((s.PartidoSolicitanteId == partido1Id && s.PartidoReceptorId == partido2Id)
                    || (s.PartidoSolicitanteId == partido2Id && s.PartidoReceptorId == partido1Id)));
        }

        public async Task<List<SolicitudAlianza>> GetSolicitudesPendientesParaPartido(int partidoId)
        {
            return await _context.SolicitudesAlianza
                .Include(s => s.PartidoSolicitante)
                .Include(s => s.PartidoReceptor)
                .Where(s => s.PartidoReceptorId == partidoId
                    && s.Estado == EstadoSolicitudAlianza.EnEsperaDeRespuesta)
                .ToListAsync();
        }

        public async Task<List<SolicitudAlianza>> GetSolicitudesRealizadasPorPartido(int partidoId)
        {
            return await _context.SolicitudesAlianza
                .Include(s => s.PartidoSolicitante)
                .Include(s => s.PartidoReceptor)
                .Where(s => s.PartidoSolicitanteId == partidoId)
                .ToListAsync();
        }

        public async Task<List<SolicitudAlianza>> GetAlianzasVigentes(int partidoId)
        {
            return await _context.SolicitudesAlianza
                .Include(s => s.PartidoSolicitante)
                .Include(s => s.PartidoReceptor)
                .Where(s => s.Estado == EstadoSolicitudAlianza.Aceptada
                    && (s.PartidoSolicitanteId == partidoId || s.PartidoReceptorId == partidoId))
                .ToListAsync();
        }
    }
}