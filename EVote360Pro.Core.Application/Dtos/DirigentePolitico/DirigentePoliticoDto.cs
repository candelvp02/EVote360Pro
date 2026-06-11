using EVote360Pro.Core.Application.Dtos.PartidoPolitico;
using EVote360Pro.Core.Application.Dtos.Usuario;

namespace EVote360Pro.Core.Application.Dtos.DirigentePolitico
{
    public class DirigentePoliticoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int PartidoPoliticoId { get; set; }
        public UsuarioDto? Usuario { get; set; }
        public PartidoPoliticoDto? PartidoPolitico { get; set; }
        public bool Activo { get; set; }
    }
}