using EVote360Pro.Core.Application.Dtos.Voto;
using EVote360Pro.Core.Application.ViewModels.Voto;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface IVotacionService
    {
        Task<(bool exito, string mensaje, int ciudadanoId, int eleccionId)> ValidarInicioVotacionAsync(string numeroDocumento);
        Task<bool> GenerarYEnviarCodigoAsync(int ciudadanoId, int eleccionId);
        Task<bool> ValidarCodigoAsync(int ciudadanoId, int eleccionId, string codigo);
        Task<SeleccionVotoViewModel?> GetPuestosParaVotarAsync(int ciudadanoId, int eleccionId);
        Task<bool> RegistrarVotosAsync(int ciudadanoId, int eleccionId, Dictionary<int, int?> selecciones);
        Task<List<VotoDto>> GetResumenVotosAsync(int ciudadanoId, int eleccionId);
    }
}