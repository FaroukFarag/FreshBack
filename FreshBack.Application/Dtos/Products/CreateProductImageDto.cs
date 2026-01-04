using FreshBack.Application.Dtos.Abstraction;
using Microsoft.AspNetCore.Http;

namespace FreshBack.Application.Dtos.Products;

public class CreateProductImageDto : BaseModelDto<int>
{
    public IFormFile ImageFile { get; set; } = default!;
    public int ProductId { get; set; }
}
