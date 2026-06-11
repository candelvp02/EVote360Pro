using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.Voto
{
    public class IniciarVotacionViewModel
    {
        [Required(ErrorMessage = "El número de documento es requerido")]
        public string NumeroDocumento { get; set; } = string.Empty;
    }
}