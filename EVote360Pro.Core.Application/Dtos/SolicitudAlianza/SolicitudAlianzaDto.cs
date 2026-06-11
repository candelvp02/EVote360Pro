using EVote360Pro.Core.Domain.Enums;
using EVote360Pro.Core.Application.Dtos.PartidoPolitico;

namespace EVote360Pro.Core.Application.Dtos.SolicitudAlianza
{
    public class SolicitudAlianzaDto
    {
        public int Id { get; set; }
        public int PartidoSolicitanteId { get; set; }
        public int PartidoReceptorId { get; set; }
        public PartidoPoliticoDto? PartidoSolicitante { get; set; }
        public PartidoPoliticoDto? PartidoReceptor { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public EstadoSolicitudAlianza Estado { get; set; }
        public bool Activo { get; set; }
    }
}