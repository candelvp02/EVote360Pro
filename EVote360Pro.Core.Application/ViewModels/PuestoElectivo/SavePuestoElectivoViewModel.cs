using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.PuestoElectivo
{
    public class SavePuestoElectivoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}