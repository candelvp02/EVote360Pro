using EVote360Pro.Core.Domain.Common;

namespace EVote360Pro.Core.Domain.Entities
{
    public class CodigoVerificacion
    {
        public required int CiudadanoId { get; set; }
        public required int EleccionId { get; set;
        public required string Codigo { get; set; }
        public required DateTime FechaGeneracion { get; set; }
        public required DateTime FechaExpiracion { get; set; }
        public bool Usado { get; set; }
        public Ciudadano? Ciudadano { get; set; }
        public Eleccion? Eleccion { get; set; }
    }
}