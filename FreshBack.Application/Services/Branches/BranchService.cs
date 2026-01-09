using AutoMapper;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Branches;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;
using NetTopologySuite.Geometries;

namespace FreshBack.Application.Services.Branches;

public class BranchService(
    IBranchRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        BranchDto,
        BranchDto,
        BranchDto,
        BranchDto,
        Branch,
        int>(repository, unitOfWork, mapper), IBranchService
{
    private readonly IBranchRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async override Task<ResultDto<BranchDto>> GetAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Branchs Paginated",
            action: async () =>
            {
                var spec = new BaseSpecification<Branch>
                {
                    Includes =
                    [
                        b => b.Products
                    ]
                };
                var branch = await _repository.GetAsync(id, spec);

                return _mapper.Map<BranchDto>(branch);
            });
    }

    public async Task<ResultDto<PagedResult<BranchDto>>> GetAllPaginatedAsync(BranchPaginatedModelDto paginatedModelDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get All Branches Paginated",
            action: async () =>
            {
                var location = new Point(
                    paginatedModelDto.Longitude,
                    paginatedModelDto.Latitude)
                {
                    SRID = 4326
                };
                var spec = new BaseSpecification<Branch>
                {
                    Criteria = m => m.Status == BranchStatus.Active,
                    OrderBy = m => m.Location.Distance(location)
                };
                var paginatedModel = _mapper.Map<PaginatedModel>(paginatedModelDto);
                var (branches, totalCount) = await _repository
                    .GetAllPaginatedAsync(paginatedModel, spec);

                return new PagedResult<BranchDto>(
                    _mapper.Map<IEnumerable<BranchDto>>(branches),
                    totalCount);
            });
    }
}
