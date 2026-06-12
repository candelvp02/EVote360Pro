
namespace EVote360Pro.Core.Application.Dtos.SolicitudAlianza
{
    public class SaveSolicitudAlianzaDto
    {
        public int Id { get; set; }
        public int PartidoSolicitanteId { get; set; }
        public int PartidoReceptorId { get; set; }
        public bool Activo { get; set; }
    }
}