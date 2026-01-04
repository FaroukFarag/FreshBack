using FreshBack.Application.Dtos.ProductsOrders;
using FreshBack.Application.Interfaces.ProductsOrders;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.ProductsOrders;

[Route("api/[controller]")]
[ApiController]
public class ProductsOrdersController(IProductOrderService service) :
    BaseController<IProductOrderService, CreateProductOrderDto, ProductOrderDto,
        ProductOrderDto, ProductOrderDto, ProductOrder,
        (int productId, int orderId)>(service)
{
    [HttpGet("Get")]
    public async Task<IActionResult> Get(int productId, int orderId)
    {
        var id = (productId, orderId);

        return await base.Get(id);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [NonAction]
    public override async Task<IActionResult> Get((int, int) id)
    {
        return await base.Get(id);
    }
}
