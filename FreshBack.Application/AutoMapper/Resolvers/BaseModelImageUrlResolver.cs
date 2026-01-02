using AutoMapper;
using FreshBack.Application.Configurations;
using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Models.Abstraction;
using Microsoft.Extensions.Options;

namespace FreshBack.Application.AutoMapper.Resolvers;

public class BaseModelImageUrlResolver(IOptions<ImageSettings> settings) : IValueResolver<BaseImageModelDto<int>, BaseImageModel<int>, string>
{
    private readonly ImageSettings _settings = settings.Value;

    public string Resolve(BaseImageModelDto<int> source, BaseImageModel<int> destination, string? destMember, ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source.ImagePath))
            return default!;

        return $"{source.ImagePath.Replace($"{_settings.BaseUrl.TrimEnd('/')}/", "").TrimStart('/')}";
    }
}
