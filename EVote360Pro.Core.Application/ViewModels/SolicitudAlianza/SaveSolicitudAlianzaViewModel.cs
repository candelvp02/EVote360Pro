using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.SolicitudAlianza
{
    public class SaveSolicitudAlianzaViewModel
    {
        public int Id { get; set; }
        public int PartidoSolicitanteId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un partido receptor")]
        public int PartidoReceptorId { get; set; }

        public bool Activo { get; set; }
    }
}