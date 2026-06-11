using EVote360Pro.Core.Domain.Entities;

namespace EVote360Pro.Core.Domain.Interfaces
{
    public interface IEleccionRepository : IGenericRepository<Eleccion>
    {
        Task<Eleccion?> GetEleccionActiva();
        Task<bool> ExisteEleccionActiva();
        Task<List<int>> GetAnosConElecciones();
        Task<List<Eleccion>> GetByAno(int ano);
    }
}