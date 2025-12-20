using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Abstraction;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController<TService, TCreateEntityDto, TGetAllEntitiesDto,
    TGetEntityDto, TUpdateEntityDto, TEntity, TPrimaryKey>(TService service) :
    ControllerBase
    where TCreateEntityDto : class
    where TGetAllEntitiesDto : class
    where TGetEntityDto : class
    where TUpdateEntityDto : class
    where TEntity : class
    where TService : IBaseService<TCreateEntityDto, TGetAllEntitiesDto,
        TGetEntityDto, TUpdateEntityDto, TEntity, TPrimaryKey>
{
    private readonly TService _service = service;

    [HttpPost("Create")]
    public virtual async Task<IActionResult> Create(TCreateEntityDto createEntityDto)
    {
        return Ok(await _service.CreateAsync(createEntityDto));
    }

    [HttpPost("CreateRange")]
    public virtual async Task<IActionResult> CreateRange(
        IEnumerable<TCreateEntityDto> createEntitiesDtos)
    {
        return Ok(await _service.CreateRangeAsync(createEntitiesDtos));
    }

    [HttpGet("Get")]
    public virtual async Task<IActionResult> Get(TPrimaryKey id)
    {
        var entityDto = await _service.GetAsync(id);

        if (entityDto == null)
            return NotFound();

        return Ok(entityDto);
    }

    [HttpGet("GetAll")]
    public virtual async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpPost("GetAllPaginated")]
    public virtual async Task<IActionResult> GetAllPaginated(PaginatedModelDto paginatedModelDto)
    {
        return Ok(await _service.GetAllPaginatedAsync(paginatedModelDto));
    }

    [HttpPut("Update")]
    public virtual async Task<IActionResult> Update(TUpdateEntityDto updateEntityDto)
    {
        return Ok(await _service.UpdateAsync(updateEntityDto));
    }

    [HttpPut("UpdateRange")]
    public virtual async Task<IActionResult> UpdateRange(IEnumerable<TUpdateEntityDto> updateEntitiesDtos)
    {
        return Ok(await _service.UpdateRangeAsync(updateEntitiesDtos));
    }

    [HttpDelete("Delete")]
    public virtual async Task<IActionResult> Delete(TPrimaryKey id)
    {
        var entityDto = await _service.DeleteAsync(id);

        if (entityDto == null)
            return NotFound();

        return Ok(entityDto);
    }

    [HttpDelete("DeleteRange")]
    public virtual async Task<IActionResult> DeleteRange(IEnumerable<TGetAllEntitiesDto> getAllEntitiesDtos)
    {
        return Ok(await _service.DeleteRangeAsync(getAllEntitiesDtos));
    }

    protected int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("User ID not found in claims.");
        }

        return int.Parse(userIdClaim);
    }
}
