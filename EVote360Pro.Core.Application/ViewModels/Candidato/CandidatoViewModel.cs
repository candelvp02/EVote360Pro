namespace EVote360Pro.Core.Application.ViewModels.Candidato
{
    public class CandidatoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string FotoUrl { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public string NombrePartido { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}