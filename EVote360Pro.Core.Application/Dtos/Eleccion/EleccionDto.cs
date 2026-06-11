using EVote360Pro.Core.Domain.Enums;

namespace EVote360Pro.Core.Application.Dtos.Eleccion
{
    public class EleccionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaRealizacion { get; set; }
        public EstadoEleccion Estado { get; set; }
        public bool Activo { get; set; }
    }
}