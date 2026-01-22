using FreshBack.Application.Dtos.CustomersBranchesFavorite;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.CustomersBranchesFavorite;
using FreshBack.Domain.Models.BranchesFavorites;
using FreshBack.WebApi.Controllers.Abstraction;
using FreshBack.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.CustomersBranchesFavorite;

[Route("api/[controller]")]
[ApiController]
public class CustomersBranchesFavoriteController(
    ICustomerBranchFavoriteService service) :
    BaseController<ICustomerBranchFavoriteService, CreateCustomerBranchFavoriteDto,
        CustomerBranchFavoriteDto, CustomerBranchFavoriteDto, CustomerBranchFavoriteDto,
        CustomerBranchFavorite, (int CustomerId, int BranchId)>(service)
{
    private readonly ICustomerBranchFavoriteService _service = service;
    [HttpGet("Get")]
    public async Task<IActionResult> Get(int customerId, int branchId)
    {
        var id = (customerId, branchId);

        return await base.Get(id);
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [NonAction]
    public override async Task<IActionResult> Get((int, int) id)
    {
        return await base.Get(id);
    }

    [HttpPost("GetCustomerFavoriteBranchesPaginated")]
    public async Task<IActionResult> GetCustomerFavoriteBranchesPaginated(
        PaginatedModelDto paginatedModelDto)
    {
        return Ok(await _service
            .GetCustomerFavoriteBranchesPaginatedAsync(
                paginatedModelDto,
                User.GetUserId()));
    }
}
