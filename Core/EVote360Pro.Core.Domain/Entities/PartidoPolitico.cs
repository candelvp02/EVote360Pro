using EVote360Pro.Core.Domain.Common;

namespace EVote360Pro.Core.Domain.Entities
{
    public class PartidoPolitico : BasicEntity<int>
    {
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public required string Siglas { get; set; }
        public required string LogoUrl { get; set; }

        public ICollection<Candidato>? Candidatos { get; set; }
        public DirigentePolitico? DirigentePolitico { get; set; }
        public ICollection<SolicitudAlianza>? SolicitudesEnviadas { get; set; }
        public ICollection<SolicitudAlianza>? SolicitudesRecibidas { get; set; }
        public ICollection<CandidatoPuesto>? CandidatosPuestos { get; set; }

    }
}