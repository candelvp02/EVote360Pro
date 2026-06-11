using System.ComponentModel.DataAnnotations;

namespace EVote360Pro.Core.Application.ViewModels.CandidatoPuesto
{
    public class SaveCandidatoPuestoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un candidato")]
        public int CandidatoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un puesto electivo")]
        public int PuestoElectivoId { get; set; }

        public int PartidoPoliticoId { get; set; }
        public bool Activo { get; set; }
    }
}