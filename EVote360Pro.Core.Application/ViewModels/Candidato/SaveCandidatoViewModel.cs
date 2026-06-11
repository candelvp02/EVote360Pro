using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.Candidato
{
    public class SaveCandidatoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; } = string.Empty;

        public string FotoUrl { get; set; } = string.Empty;
        public IFormFile? FotoFile { get; set; }
        public int PartidoPoliticoId { get; set; }
        public bool Activo { get; set; }
    }
}