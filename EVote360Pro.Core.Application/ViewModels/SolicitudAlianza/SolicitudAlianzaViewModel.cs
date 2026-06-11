using EVote360Pro.Core.Domain.Enums;

namespace EVote360Pro.Core.Application.ViewModels.SolicitudAlianza
{
    public class SolicitudAlianzaViewModel
    {
        public int Id { get; set; }
        public int PartidoSolicitanteId { get; set; }
        public string NombrePartidoSolicitante { get; set; } = string.Empty;
        public int PartidoReceptorId { get; set; }
        public string NombrePartidoReceptor { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public EstadoSolicitudAlianza Estado { get; set; }
        public string NombreEstado => Estado switch
        {
            EstadoSolicitudAlianza.EnEsperaDeRespuesta => "En Espera",
            EstadoSolicitudAlianza.Aceptada => "Aceptada",
            EstadoSolicitudAlianza.Rechazada => "Rechazada",
            _ => "Desconocido"
        };
        public bool Activo { get; set; }
    }
}