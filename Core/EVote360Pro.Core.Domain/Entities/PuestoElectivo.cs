using EVote360Pro.Core.Domain.Common;

namespace EVote360Pro.Core.Domain.Entities
{
    public class PuestoElectivo : BasicEntity<int>
    {
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }

        public ICollection<CandidatoPuesto>? CandidatoPuestos { get; set; }
        public ICollection<Voto>? Votos { get; set; }
    }
}