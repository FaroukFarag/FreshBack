using AutoMapper;
using FreshBack.Application.Configurations;
using FreshBack.Application.Helpers;
using Microsoft.Extensions.Options;

namespace FreshBack.Application.AutoMapper.Resolvers;

public class ImagePathToUrlResolver<TSource, TDestination>(
    IOptions<ImageSettings> settings)
    : IValueResolver<TSource, TDestination, string?>
    where TSource : class
    where TDestination : class
{
    private readonly ImageSettings _settings = settings.Value;

    public string? Resolve(TSource source, TDestination destination, string? destMember, ResolutionContext context)
    {
        var imagePathProperty = source.GetType().GetProperty("ImagePath");
        if (imagePathProperty == null)
            return null;

        var imagePath = imagePathProperty.GetValue(source) as string;

        return ImagePathHelper.ToFullUrl(imagePath, _settings.BaseUrl);
    }
}
