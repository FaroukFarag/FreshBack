using FreshBack.Application.Dtos.Shared;

namespace FreshBack.Application.Interfaces.Abstraction;

public interface IBaseService<
    TCreateEntityDto, TGetAllEntitiesDto, TGetEntityDto,
    TUpdateEntityDto, TEntity, TPrimaryKey>
{
    Task<ResultDto<TCreateEntityDto>> CreateAsync(TCreateEntityDto createEntityDto);
    Task<ResultDto<bool>> CreateRangeAsync(IEnumerable<TCreateEntityDto> createEntitiesDtos);
    Task<ResultDto<TGetEntityDto>> GetAsync(TPrimaryKey id);
    Task<ResultDto<IEnumerable<TGetAllEntitiesDto>>> GetAllAsync();
    Task<ResultDto<IEnumerable<TGetAllEntitiesDto>>> GetAllPaginatedAsync(PaginatedModelDto paginatedModelDto);
    Task<ResultDto<IEnumerable<TGetAllEntitiesDto>>> GetAllFilteredAsync<TFilterDto>(TFilterDto filterDto);
    Task<ResultDto<TUpdateEntityDto>> UpdateAsync(TUpdateEntityDto updateEntityDto);
    Task<ResultDto<bool>> UpdateRangeAsync(IEnumerable<TUpdateEntityDto> updateEntitiesDtos);
    Task<ResultDto<TGetEntityDto>> DeleteAsync(TPrimaryKey id);
    Task<ResultDto<bool>> DeleteRangeAsync(IEnumerable<TGetAllEntitiesDto> getAllEntitiesDtos);
}
