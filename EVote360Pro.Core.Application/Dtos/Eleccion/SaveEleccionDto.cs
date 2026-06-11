
namespace EVote360Pro.Core.Application.Dtos.Eleccion
{
    internal class SaveEleccionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaRealizacion { get; set; }
        public bool Activo { get; set; }
    }
}