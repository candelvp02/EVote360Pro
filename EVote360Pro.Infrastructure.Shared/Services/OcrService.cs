using EVote360Pro.Core.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Tesseract;

namespace EVote360Pro.Infrastructure.Shared.Services
{
    public class OcrService : IOcrService
    {
        private readonly string _tessDataPath;

        public OcrService(string tessDataPath)
        {
            _tessDataPath = tessDataPath;
        }

        public async Task<string> ExtraerTextoAsync(IFormFile imagen)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await imagen.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                using var engine = new TesseractEngine(_tessDataPath, "spa", EngineMode.Default);
                using var img = Pix.LoadFromMemory(imageBytes);
                using var page = engine.Process(img);

                return page.GetText() ?? string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public bool ValidarDocumento(string textoExtraido, string numeroDocumento)
        {
            if (string.IsNullOrWhiteSpace(textoExtraido) || string.IsNullOrWhiteSpace(numeroDocumento))
                return false;

            string textoLimpio = textoExtraido
                .Replace("-", "")
                .Replace(" ", "")
                .Replace("\n", "")
                .Replace("\r", "");

            string documentoLimpio = numeroDocumento
                .Replace("-", "")
                .Replace(" ", "");

            return textoLimpio.Contains(documentoLimpio, StringComparison.OrdinalIgnoreCase);
        }
    }
}