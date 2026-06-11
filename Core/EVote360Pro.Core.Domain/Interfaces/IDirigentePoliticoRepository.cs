using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface IDirigentePoliticoRepository : IGenericRepository<DirigentePolitico>
    {
        Task<DirigentePolitico?> GetByUsuarioId(int usuarioId);
        Task<DirigentePolitico?> GetByPartidoId(int partidoId);
        Task<bool> UsuarioYaAsignado(int usuarioId);
        Task<bool> PartidoYaAsignado(int partidoId);
    }
}