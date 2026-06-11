namespace EVote360Pro.Core.Application.ViewModels.PartidoPolitico
{
    public class PartidoPoliticoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Siglas { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}