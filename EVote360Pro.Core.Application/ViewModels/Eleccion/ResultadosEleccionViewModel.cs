namespace EVote360Pro.Core.Application.ViewModels.Eleccion
{
    public class ResultadosEleccionViewModel
    {
        public int EleccionId { get; set; }
        public string NombreEleccion { get; set; } = string.Empty;
        public List<ResultadoPuestoViewModel> ResultadosPorPuesto { get; set; } = new();
    }

    public class ResultadoPuestoViewModel
    {
        public int PuestoElectivoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public List<ResultadoCandidatoViewModel> ResultadosCandidatos { get; set; } = new();
        public bool HayEmpate { get; set; }
        public string? GanadorNombre { get; set; }
    }

    public class ResultadoCandidatoViewModel
    {
        public int? CandidatoId { get; set; }
        public string NombreCandidato { get; set; } = string.Empty;
        public string NombrePartido { get; set; } = string.Empty;
        public int TotalVotos { get; set; }
        public double Porcentaje { get; set; }
        public bool EsNinguno { get; set; }
    }
}