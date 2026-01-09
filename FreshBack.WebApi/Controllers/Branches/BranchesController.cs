using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Domain.Models.Branches;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Branches;

[Route("api/[controller]")]
[ApiController]
public class BranchesController(IBranchService service) :
    BaseController<IBranchService, BranchDto, BranchDto, BranchDto, BranchDto, Branch,
        int>(service)
{
    private readonly IBranchService _service = service;

    [HttpPost("GetAllBranchesPaginated")]
    public async Task<IActionResult> GetAllPaginated(
        BranchPaginatedModelDto paginatedModelDto)
    {
        return Ok(await _service.GetAllPaginatedAsync(paginatedModelDto));
    }
}
