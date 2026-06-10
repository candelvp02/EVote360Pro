using EVote360Pro.Core.Domain.Common;
using EVote360Pro.Core.Domain.Enums;

namespace EVote360Pro.Core.Domain.Entities
{
    public class Usuario : BasicEntity<int>
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string CorreoElectronico { get; set; }
        public required string NombreUsuario { get; set; }
        public required string Contrasena { get; set; }
        public required RolUsuario Rol { get; set; }

        public DirigentePolitico? DirigentePolitico { get; set; }
    }
}
