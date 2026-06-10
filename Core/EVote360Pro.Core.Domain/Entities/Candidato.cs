using EVote360Pro.Core.Domain.Common;

namespace EVote360Pro.Core.Domain.Entities
{
    public class Candidato : BasicEntity<int>
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string FotoUrl { get; set; }
        public required int PartidoPoliticoId { get; set; }

        public PartidoPolitico? PartidoPolitico { get; set; }
        public ICollection<CandidatoPuesto>? CandidatoPuestos { get; set; }
        public ICollection<Voto>? Votos { get; set; }
    }
}