using EVote360Pro.Core.Application.Dtos.SolicitudAlianza;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface ISolicitudAlianzaService
    {
        Task<SolicitudAlianzaDto?> CrearSolicitudAsync(SaveSolicitudAlianzaDto dto);
        Task<bool> AceptarSolicitudAsync(int id);
        Task<bool> RechazarSolicitudAsync(int id);
        Task<bool> EliminarSolicitudAsync(int id);
        Task<bool> EliminarAlianzaAsync(int id);
        Task<List<SolicitudAlianzaDto>> GetSolicitudesPendientesParaPartido(int partidoId);
        Task<List<SolicitudAlianzaDto>> GetSolicitudesRealizadasPorPartido(int partidoId);
        Task<List<SolicitudAlianzaDto>> GetAlianzasVigentes(int partidoId);
    }
}