using EVote360Pro.Core.Domain.Enums;

namespace EVote360Pro.Core.Application.Dtos.Usuario
{
    public class SaveUsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string ConfirmacionContrasena { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
        public bool Activo { get; set; }
    }
}