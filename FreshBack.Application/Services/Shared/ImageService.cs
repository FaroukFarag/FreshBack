using FreshBack.Application.Interfaces.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FreshBack.Application.Services.Shared;

public class ImageService(ILogger<ImageService> logger) : IImageService
{
    private readonly ILogger<ImageService> _logger = logger;
    private const string BaseImagePath = "images";

    public async Task<string> SaveImageAsync(IFormFile? imageFile, string subFolder)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            throw new ArgumentException("Image file is empty or null", nameof(imageFile));
        }

        try
        {
            var uploadsFolder = Path.Combine(BaseImagePath, subFolder);

            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return filePath;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving image");

            throw;
        }
    }

    public void DeleteImageAsync(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return;
        }

        try
        {
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting image: {ImagePath}", imagePath);

            throw;
        }
    }
}