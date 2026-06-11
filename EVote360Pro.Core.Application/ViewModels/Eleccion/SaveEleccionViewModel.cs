using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.Eleccion
{
    public class SaveEleccionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de realización es requerida")]
        public DateTime FechaRealizacion { get; set; }

        public bool Activo { get; set; }
    }
}