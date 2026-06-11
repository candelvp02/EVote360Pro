
namespace EVote360Pro.Core.Application.Dtos.Voto
{
    public class VotoDto
    {
        public int Id { get; set; }
        public int CiudadanoId { get; set; }
        public int EleccionId { get; set; }
        public int PuestoElectivoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public int? CandidatoId { get; set; }
        public string NombreCandidato { get; set; } = string.Empty;
        public string NombrePartido { get; set; } = string.Empty;
        public bool EsNinguno { get; set; }
        public bool Finalizado { get; set; }
        public bool Activo { get; set; }
    }
}