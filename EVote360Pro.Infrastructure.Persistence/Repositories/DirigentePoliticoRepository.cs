using EVote360Pro.Core.Domain.Entities;
using EVote360Pro.Core.Domain.Interfaces;
using EVote360Pro.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EVote360Pro.Infrastructure.Persistence.Repositories
{
    public class DirigentePoliticoRepository : GenericRepository<DirigentePolitico>, IDirigentePoliticoRepository
    {
        private readonly EVoteContext _context;

        public DirigentePoliticoRepository(EVoteContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DirigentePolitico?> GetByUsuarioId(int usuarioId)
        {
            return await _context.DirigentesPoliticos
                .Include(d => d.Usuario)
                .Include(d => d.PartidoPolitico)
                .FirstOrDefaultAsync(d => d.UsuarioId == usuarioId);
        }

        public async Task<DirigentePolitico?> GetByPartidoId(int partidoId)
        {
            return await _context.DirigentesPoliticos
                .Include(d => d.Usuario)
                .Include(d => d.PartidoPolitico)
                .FirstOrDefaultAsync(d => d.PartidoPoliticoId == partidoId);
        }

        public async Task<bool> UsuarioYaAsignado(int usuarioId)
        {
            return await _context.DirigentesPoliticos
                .AnyAsync(d => d.UsuarioId == usuarioId);
        }

        public async Task<bool> PartidoYaAsignado(int partidoId)
        {
            return await _context.DirigentesPoliticos
                .AnyAsync(d => d.PartidoPoliticoId == partidoId);
        }
    }
}