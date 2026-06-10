using EVote360Pro.Core.Domain.Common;
using EVote360Pro.Core.Domain.Enums;

namespace EVote360Pro.Core.Domain.Entities
{
    public class Eleccion : BasicEntity<int>
    {
        public required string Nombre { get; set; }
        public required DateTime FechaRealizacion { get; set; }
        public required EstadoEleccion Estado { get; set; }
        public ICollection<Voto>? Votos { get; set; }
    }
}