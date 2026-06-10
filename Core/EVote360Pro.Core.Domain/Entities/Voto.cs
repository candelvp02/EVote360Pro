using EVote360Pro.Core.Domain.Common;

namespace EVote360Pro.Core.Domain.Entities
{
    public class Voto : BasicEntity<int>
    {
        public required int CiudadanoId { get; set; }
        public required int EleccionId { get; set; }
        public required int PuestoElectivoId { get; set; }
        public int? CandidaatoId { get; set; }
        public bool EsNinguno { get; set; }
        public bool Finalizado { get; set; }
        public Ciudadano? Ciudadano { get; set; }
        public Eleccion? Eleccion { get; set; }
        public PuestoElectivo? PuestoElectivo { get; set; }
        public Candidato? Candidato { get; set; }
    }
}