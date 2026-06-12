
namespace EVote360Pro.Core.Application.Dtos.CandidatoPuesto
{
    public class SaveCandidatoPuestoDto
    {
        public int Id { get; set; }
        public int CandidatoId { get; set; }
        public int PuestoElectivoId { get; set; }
        public int PartidoPoliticoId { get; set; }
        public bool Activo { get; set; }
    }
}