using Microsoft.AspNetCore.Http;
using TechMove.Api.Services.Interfaces;

namespace TechMove.Api.Services.Implementations
{
    public class FileService : IFileService
    {
        public async Task<string> SavePdfAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is required");

            if (!file.FileName.EndsWith(".pdf"))
                throw new Exception("Only PDF allowed");

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}