using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.Voto
{
    public class IniciarVotacionViewModel
    {
        [Required(ErrorMessage = "El número de documento es requerido")]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe cargar una imagen de su cédula")]
        public IFormFile? ImagenCedula { get; set; }
    }
}