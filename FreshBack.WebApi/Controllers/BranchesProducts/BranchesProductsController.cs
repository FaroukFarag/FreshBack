using FreshBack.Application.Dtos.BranchesProducts;
using FreshBack.Application.Interfaces.BranchesProducts;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.BranchesProducts;

[Route("api/[controller]")]
[ApiController]
public class BranchesProductsController(IBranchProductService service) :
    BaseController<IBranchProductService, CreateBranchProductDto,
        BranchProductDto, BranchProductDto, BranchProductDto, BranchProduct,
        (int BranchId, int ProductId)>(service)
{
    [HttpGet("Get")]
    public async Task<IActionResult> Get(int branchId, int productId)
    {
        var id = (branchId, productId);

        return await base.Get(id);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [NonAction]
    public override async Task<IActionResult> Get((int, int) id)
    {
        return await base.Get(id);
    }
}
