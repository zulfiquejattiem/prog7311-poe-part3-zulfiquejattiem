using Microsoft.AspNetCore.Http;

namespace TechMove.Api.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> SavePdfAsync(IFormFile file);
    }
}