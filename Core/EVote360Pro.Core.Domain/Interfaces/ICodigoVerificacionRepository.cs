using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface ICodigoVerificacionRepository : IGenericRepository<CodigoVerificacion>
    {
        Task<CodigoVerificacion?> GetUltimoVigente(int ciudadanoId, int eleccionId);
    }
}