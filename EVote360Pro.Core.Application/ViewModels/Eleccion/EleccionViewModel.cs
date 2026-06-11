using EVote360Pro.Core.Domain.Enums;

namespace EVote360Pro.Core.Application.ViewModels.Eleccion
{
    public class EleccionViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaRealizacion { get; set; }
        public EstadoEleccion Estado { get; set; }
        public string NombreEstado => Estado switch
        {
            EstadoEleccion.Pendiente => "Pendiente",
            EstadoEleccion.Activa => "Activa",
            EstadoEleccion.Finalizada => "Finalizada",
            _ => "Desconocido"
        };
        public bool Activo { get; set; }
    }
}