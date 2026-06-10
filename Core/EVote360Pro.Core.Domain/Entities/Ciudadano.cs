using EVote360Pro.Core.Domain.Common;

namespace EVote360Pro.Core.Domain.Entities
{
    public class Ciudadano : BasicEntity<int>
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string CorreoElectronico { get; set; }
        public required string NumeroDocumento { get; set; }

        public ICollection<CodigoVerificacion>? CodigoVerificacion { get; set; }
        public ICollection<Voto>? Votos { get; set; }
    }
}