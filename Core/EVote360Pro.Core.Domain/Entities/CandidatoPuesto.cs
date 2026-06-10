using EVote360Pro.Core.Domain.Common;

namespace EVote360Pro.Core.Domain.Entities
{
    public class CandidatoPuesto : BasicEntity<int>
    {
        public required int CandidatoId { get; set; }
        public required int PuestoElectivoId { get; set; }
        public Candidato? Candidato { get; set; }
        public PuestoElectivo? PuestoElectivo { get; set; }
        public PartidoPolitico? PartidoPolitico { get; set; }
    }
}