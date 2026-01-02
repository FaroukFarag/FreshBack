using AutoMapper;
using FreshBack.Application.Configurations;
using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Models.Abstraction;
using Microsoft.Extensions.Options;

namespace FreshBack.Application.AutoMapper.Resolvers;

public class BaseModelImageDtoUrlResolver(IOptions<ImageSettings> settings) : IValueResolver<BaseImageModel<int>, BaseImageModelDto<int>, string?>
{
    private readonly ImageSettings _settings = settings.Value;

    public string? Resolve(BaseImageModel<int> source, BaseImageModelDto<int> destination, string? destMember, ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source.ImagePath))
            return null;

        return $"{_settings.BaseUrl.TrimEnd('/')}/{source.ImagePath.Replace("\\", "/").TrimStart('/')}";
    }
}
