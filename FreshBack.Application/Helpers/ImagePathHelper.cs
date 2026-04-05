namespace FreshBack.Application.Helpers;

public static class ImagePathHelper
{
    public static string? ToFullUrl(string? imagePath, string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return null;

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("Base URL cannot be empty", nameof(baseUrl));

        var normalizedBaseUrl = baseUrl.TrimEnd('/');
        var normalizedPath = imagePath.Replace("\\", "/").TrimStart('/');

        return $"{normalizedBaseUrl}/{normalizedPath}";
    }

    public static string? ToRelativePath(string? imageUrl, string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return null;

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("Base URL cannot be empty", nameof(baseUrl));

        var normalizedBaseUrl = baseUrl.TrimEnd('/') + "/";

        if (!imageUrl.StartsWith(normalizedBaseUrl, StringComparison.OrdinalIgnoreCase))
            return imageUrl.Replace("\\", "/").TrimStart('/');

        var relativePath = imageUrl.Replace(normalizedBaseUrl, "").TrimStart('/');

        return relativePath;
    }
}
