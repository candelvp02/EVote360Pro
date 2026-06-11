using EVote360Pro.Core.Domain.Enums;

namespace EVote360Pro.Core.Application.ViewModels.Usuario
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
        public string NombreRol => Rol == RolUsuario.Administrador ? "Administrador" : "Dirigente Político";
        public bool Activo { get; set; }
    }
}