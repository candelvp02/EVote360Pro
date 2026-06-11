namespace EVote360Pro.Core.Application.ViewModels.DirigentePolitico
{
    public class DirigentePoliticoViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreCompletoUsuario { get; set; } = string.Empty;
        public int PartidoPoliticoId { get; set; }
        public string NombrePartido { get; set; } = string.Empty;
        public string SiglasPartido { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}