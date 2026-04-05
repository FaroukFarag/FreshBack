using FreshBack.Application.Dtos.BranchesProducts;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.BranchesProducts;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.BranchesProducts;

[Route("api/[controller]")]
[ApiController]
public class BranchesProductsController(IBranchProductService service) :
    BaseController<IBranchProductService, CreateBranchProductDto,
        BranchProductDto, BranchProductDto, CreateBranchProductDto, BranchProduct,
        (int BranchId, int ProductId)>(service)
{
    private readonly IBranchProductService _service = service;

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

    [HttpPost("GetBranchesRemainingProductsPaginated")]
    public async Task<IActionResult> GetBranchRemainingProductsPaginated(
        PaginatedModelDto paginatedModelDto)
    {
        return Ok(await _service
            .GetBranchesRemainingProductsPaginatedAsync(paginatedModelDto));
    }
}
