using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.Usuario
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;
    }
}