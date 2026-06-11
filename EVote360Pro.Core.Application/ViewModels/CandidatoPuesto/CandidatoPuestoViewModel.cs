namespace EVote360Pro.Core.Application.ViewModels.CandidatoPuesto
{
    public class CandidatoPuestoViewModel
    {
        public int Id { get; set; }
        public int CandidatoId { get; set; }
        public string NombreCandidato { get; set; } = string.Empty;
        public int PuestoElectivoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public string NombrePartido { get; set; } = string.Empty;
        public bool EsCandidatoAliado { get; set; }
        public bool Activo { get; set; }
    }
}