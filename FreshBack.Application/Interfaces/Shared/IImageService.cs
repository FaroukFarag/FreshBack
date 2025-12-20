using Microsoft.AspNetCore.Http;

namespace FreshBack.Application.Interfaces.Shared;

public interface IImageService
{
    Task<string> SaveImageAsync(IFormFile? imageFile, string subFolder);
    void DeleteImageAsync(string imagePath);
}
