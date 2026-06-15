namespace EVote360Pro.Helpers
{
    public static class FileManager
    {
        public static string Upload(IFormFile? file, int id, string folder)
        {
            if (file == null) return string.Empty;

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", folder);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string extension = Path.GetExtension(file.FileName);
            string fileName = $"{folder}_{id}{extension}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return $"/img/{folder}/{fileName}";
        }

        public static void Delete(string imageUrl, string folder)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}