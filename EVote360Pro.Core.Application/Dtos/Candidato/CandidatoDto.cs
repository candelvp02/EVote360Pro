using EVote360Pro.Core.Application.Dtos.PartidoPolitico;

namespace EVote360Pro.Core.Application.Dtos.Candidato
{
    public class CandidatoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string FotoUrl { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public PartidoPoliticoDto? PartidoPolitico { get; set; }
        public bool Activo { get; set; }
    }
}