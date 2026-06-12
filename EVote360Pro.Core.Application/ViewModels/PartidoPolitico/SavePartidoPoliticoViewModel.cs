using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.PartidoPolitico
{
    public class SavePartidoPoliticoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Las siglas son requeridas")]
        public string Siglas { get; set; } = string.Empty;

        public string LogoUrl { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}