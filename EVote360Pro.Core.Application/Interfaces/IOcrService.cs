using Microsoft.AspNetCore.Http;

namespace EVote360Pro.Core.Application.Interfaces
{
    public interface IOcrService
    {
        Task<string> ExtraerTextoAsync(IFormFile imagen);
        bool ValidarDocumento(string textoExtraido, string numeroDocumento);
    }
}