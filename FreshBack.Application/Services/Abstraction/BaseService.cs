using AutoMapper;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Shared;

namespace FreshBack.Application.Services.Abstraction;

public class BaseService<
    TCreateEntityDto, TGetAllEntitiesDto, TGetEntityDto,
    TUpdateEntityDto, TEntity, TPrimaryKey>(
    IBaseRepository<TEntity, TPrimaryKey> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IBaseService<
        TCreateEntityDto, TGetAllEntitiesDto, TGetEntityDto,
        TUpdateEntityDto, TEntity, TPrimaryKey>
    where TCreateEntityDto : class
    where TGetAllEntitiesDto : class
    where TGetEntityDto : class
    where TUpdateEntityDto : class
    where TEntity : class
{
    private readonly IBaseRepository<TEntity, TPrimaryKey> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public virtual async Task<ResultDto<TCreateEntityDto>> CreateAsync(TCreateEntityDto createEntityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Create {typeof(TEntity).Name}",
            action: async () =>
            {
                var entity = _mapper.Map<TEntity>(createEntityDto);

                await _repository.CreateAsync(entity);
                await _unitOfWork.Complete();

                return createEntityDto;
            });
    }

    public virtual async Task<ResultDto<bool>> CreateRangeAsync(IEnumerable<TCreateEntityDto> createEntitiesDtos)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Create multiple {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = _mapper.Map<IReadOnlyList<TEntity>>(createEntitiesDtos);

                await _repository.CreateRangeAsync(entities);

                return await _unitOfWork.Complete();
            });
    }

    public virtual async Task<ResultDto<TGetEntityDto>> GetAsync(TPrimaryKey id)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Get {typeof(TEntity).Name} by ID",
            action: async () =>
            {
                var entity = await _repository.GetAsync(id);

                return _mapper.Map<TGetEntityDto>(entity);
            });
    }

    public virtual async Task<ResultDto<IEnumerable<TGetAllEntitiesDto>>> GetAllAsync()
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Get all {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = await _repository.GetAllAsync();

                return _mapper.Map<IEnumerable<TGetAllEntitiesDto>>(entities);
            });
    }

    public virtual async Task<ResultDto<PagedResult<TGetAllEntitiesDto>>>
        GetAllPaginatedAsync(PaginatedModelDto paginatedModelDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Get paginated {typeof(TEntity).Name}",
            action: async () =>
            {
                var (entities, totalCount) = await _repository.GetAllPaginatedAsync(_mapper.Map<PaginatedModel>(paginatedModelDto));

                return new PagedResult<TGetAllEntitiesDto>(
                    _mapper.Map<IEnumerable<TGetAllEntitiesDto>>(entities),
                    totalCount);
            });
    }

    public virtual async Task<ResultDto<IEnumerable<TGetAllEntitiesDto>>> GetAllFilteredAsync<TFilterDto>(TFilterDto filterDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Get filtered {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = await _repository.GetAllFilteredAsync(filterDto);

                return _mapper.Map<IEnumerable<TGetAllEntitiesDto>>(entities);
            });
    }

    public virtual async Task<ResultDto<TUpdateEntityDto>> UpdateAsync(TUpdateEntityDto updateEntityDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Update {typeof(TEntity).Name}",
            action: async () =>
            {
                var entity = _mapper.Map<TEntity>(updateEntityDto);

                _repository.Update(entity);

                await _unitOfWork.Complete();

                return _mapper.Map<TUpdateEntityDto>(entity);
            });
    }

    public virtual async Task<ResultDto<bool>> UpdateRangeAsync(IEnumerable<TUpdateEntityDto> updateEntitiesDtos)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Update multiple {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = _mapper.Map<IReadOnlyList<TEntity>>(updateEntitiesDtos);

                _repository.UpdateRange(entities);

                return await _unitOfWork.Complete();
            });
    }

    public virtual async Task<ResultDto<TGetEntityDto>> DeleteAsync(TPrimaryKey id)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Delete {typeof(TEntity).Name} by ID",
            action: async () =>
            {
                var entity = _repository.Delete(id);

                await _unitOfWork.Complete();

                return _mapper.Map<TGetEntityDto>(entity);
            });
    }

    public virtual async Task<ResultDto<bool>> DeleteRangeAsync(IEnumerable<TGetAllEntitiesDto> getAllEntitiesDtos)
    {
        return await ExecuteServiceCallAsync(
            operationName: $"Delete multiple {typeof(TEntity).Name}",
            action: async () =>
            {
                var entities = _mapper.Map<IReadOnlyList<TEntity>>(getAllEntitiesDtos);

                _repository.DeleteRange(entities);

                return await _unitOfWork.Complete();
            });
    }

    protected async Task<ResultDto<T>> ExecuteServiceCallAsync<T>(
        string operationName,
        Func<Task<T>> action)
    {
        try
        {
            var result = await action();

            return ResultDto<T>.CreateSuccessResult(result);
        }

        catch (Exception ex)
        {
            return ResultDto<T>.CreateFailResult($"{operationName} failed: {ex.Message}");
        }
    }
}
