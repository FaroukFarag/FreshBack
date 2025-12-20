using Microsoft.AspNetCore.Http;

namespace FreshBack.Application.Dtos.Abstraction;

public abstract class BaseImageModelDto<T> : BaseModelDto<T>
{
    public string? ImagePath { get; set; }
    public IFormFile? ImageFile { get; set; }
}
