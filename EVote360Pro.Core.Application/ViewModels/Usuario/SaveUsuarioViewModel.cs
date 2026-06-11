using EVote360Pro.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.Usuario
{
    public class SaveUsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido")]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string ConfirmacionContrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido")]
        public RolUsuario Rol { get; set; }

        public bool Activo { get; set; }
    }
}