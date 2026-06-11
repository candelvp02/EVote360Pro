using EVote360Pro.Core.Application.Dtos.Candidato;
using EVote360Pro.Core.Application.Dtos.PartidoPolitico;
using EVote360Pro.Core.Application.Dtos.PuestoElectivo;

namespace EVote360Pro.Core.Application.Dtos.CandidatoPuesto
{
    public class CandidatoPuestoDto
    {
        public int Id { get; set; }
        public int CandidatoId { get; set; }
        public int PuestoElectivoId { get; set; }
        public int PartidoPoliticoId { get; set; }
        public CandidatoDto? Candiato { get; set; }
        public PuestoElectivoDto? PuestoElectivo { get; set; }
        public PartidoPoliticoDto? PartidoPolitico { get; set; }
        public bool Activo { get; set; }
    }
}