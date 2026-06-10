using EVote360Pro.Core.Domain.Common;

namespace EVote360Pro.Core.Domain.Entities
{
    public class DirigentePolitico : BasicEntity<int>
    {
        public required int UsuarioId { get; set; }
        public required int PartidoPoliticoId { get; set; }
        public Usuario? Usuario { get; set; }
        public PartidoPolitico? PartidoPolitico { get; set; }
    }
}