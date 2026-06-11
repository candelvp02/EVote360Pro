namespace EVote360Pro.Core.Application.ViewModels.Voto
{
    public class SeleccionVotoViewModel
    {
        public int CiudadanoId { get; set; }
        public int EleccionId { get; set; }
        public string NombreCiudadano { get; set; } = string.Empty;
        public List<PuestoVotacionViewModel> PuestosElectivos { get; set; } = new();
    }

    public class PuestoVotacionViewModel
    {
        public int PuestoElectivoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public int? CandidatoSeleccionadoId { get; set; }
        public bool SeleccionoNinguno { get; set; }
        public List<CandidatoVotacionViewModel> Candidatos { get; set; } = new();
    }

    public class CandidatoVotacionViewModel
    {
        public int CandidatoId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string FotoUrl { get; set; } = string.Empty;
        public string NombrePartido { get; set; } = string.Empty;
        public string LogoPartido { get; set; } = string.Empty;
    }
}