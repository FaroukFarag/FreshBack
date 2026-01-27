using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Domain.Models.Branches;
using FreshBack.WebApi.Controllers.Abstraction;
using FreshBack.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Branches;

[Route("api/[controller]")]
[ApiController]
public class BranchesController(IBranchService service) :
    BaseController<IBranchService, CreateBranchDto, BranchDto, BranchDto, BranchDto, Branch,
        int>(service)
{
    private readonly IBranchService _service = service;

    [HttpPost("Create")]
    public override Task<IActionResult> Create(
        [FromForm] CreateBranchDto createEntityDto)
    {
        return base.Create(createEntityDto);
    }

    [HttpPost("GetAllBranchesPaginated")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllBranchesPaginated(
        BranchPaginatedModelDto paginatedModelDto)
    {
        return Ok(await _service.GetAllPaginatedAsync(paginatedModelDto, User.GetUserId()));
    }

    [HttpPost("GetOtherBranchesPaginated")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOtherBranchesPaginated(
        OtherBranchesPaginatedModelDto paginatedModelDto)
    {
        return Ok(await _service.GetOtherBranchesPaginatedAsync(paginatedModelDto));
    }
}
