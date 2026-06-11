
namespace EVote360Pro.Core.Application.Dtos.DirigentePolitico
{
    public class SaveDirigentePoliticoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int PartidoPoliticoId { get; set; }
        public bool Activo { get; set; }
    }
}