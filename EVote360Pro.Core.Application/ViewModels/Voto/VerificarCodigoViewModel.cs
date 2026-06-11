using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.Voto
{
    public class VerificarCodigoViewModel
    {
        public int CiudadanoId { get; set; }
        public int EleccionId { get; set; }

        [Required(ErrorMessage = "El código de verificación es requerido")]
        public string Codigo { get; set; } = string.Empty;
    }
}