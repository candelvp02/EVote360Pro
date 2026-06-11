
namespace EVote360Pro.Core.Application.Dtos.Candidato
{
    public class SaveCandidatoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string FotoUrl { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public bool Activo { get; set; }
    }
}