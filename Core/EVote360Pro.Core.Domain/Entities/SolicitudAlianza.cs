using EVote360Pro.Core.Domain.Common;
using EVote360Pro.Core.Domain.Enums;

namespace EVote360Pro.Core.Domain.Entities
{
    public class SolicitudAlianza : BasicEntity<int>
    {
        public required int PartidoSolicitanteId { get; set; }
        public required int PartidoReceptorId { get; set; }
        public required DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public required EstadoSolicitudAlianza Estado { get; set; }
        public PartidoPolitico? PartidoSolicitante { get; set; }
        public PartidoPolitico? PartidoReceptor { get; set; }
    }
}