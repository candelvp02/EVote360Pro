using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.DirigentePolitico
{
    public class SaveDirigentePoliticoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El usuario dirigente es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El partido político es requerido")]
        public int PartidoPoliticoId { get; set; }
        public bool Activo { get; set; }
    }
}