using AutoMapper;
using FreshBack.Application.Configurations;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Application.Interfaces.Shared;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Common.Extensions;
using FreshBack.Domain.Constants.Branches;
using FreshBack.Domain.Interfaces.Repositories.Branches;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;
using FreshBack.Domain.Specifications.Branches;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using System.Linq.Expressions;

namespace FreshBack.Application.Services.Branches;

public class BranchService(
    IBranchRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService,
    IOptions<ImageSettings> settings)
    : BaseService<
        CreateBranchDto,
        BranchDto,
        BranchDto,
        BranchDto,
        Branch,
        int>(repository, unitOfWork, mapper),
      IBranchService
{
    private readonly IBranchRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly ImageSettings _settings = settings.Value;

    public override async Task<ResultDto<CreateBranchDto>> CreateAsync(
        CreateBranchDto createBranchDto)
    {
        return await ExecuteServiceCallAsync(
            "Create Branch",
            async () =>
            {
                createBranchDto.ImagePath =
                    await _imageService.SaveImageAsync(
                        createBranchDto.ImageFile,
                        BranchConstants.SubFolder);

                var branch = _mapper.Map<Branch>(createBranchDto);

                await _repository.CreateAsync(branch);

                if (!await _unitOfWork.Complete())
                    throw new Exception("Failed to create branch");

                return _mapper.Map<CreateBranchDto>(branch);
            });
    }

    public override async Task<ResultDto<BranchDto>> GetAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            "Get Branch",
            async () =>
            {
                var spec = new BaseSpecification<Branch>
                {
                    Includes =
                    [
                        b => b.Merchant,
                        b => b.Reviews
                    ],
                    IncludeChains =
                    [
                        new IncludeChain<Branch>
                        {
                            InitialInclude = b => b.BranchesProducts,
                            ThenIncludes =
                            [
                                bp => (bp as BranchProduct)!.Product
                            ]
                        }
                    ]
                };
                var branch = await _repository.GetAsync(id, spec);

                return _mapper.Map<BranchDto>(branch);
            });
    }

    public async Task<ResultDto<PagedResult<BranchDto>>> GetAllPaginatedAsync(
        BranchPaginatedModelDto paginatedModelDto, int customerId)
    {
        return await ExecuteServiceCallAsync(
            "Get All Branches Paginated",
            async () =>
            {
                var userLocation = CreateUserLocation(paginatedModelDto);

                var spec = new BranchPaginatedSpecification(
                    paginatedModelDto.SearchTerm,
                    paginatedModelDto.CategoryId,
                    paginatedModelDto.SortBy,
                    paginatedModelDto.SortDirection,
                    userLocation);

                var paginatedModel =
                    _mapper.Map<PaginatedModel>(paginatedModelDto);
                var projection = ToDto(userLocation, _settings.BaseUrl, customerId);

                var (branches, totalCount) =
                    await _repository.GetAllPaginatedAsync(
                        paginatedModel,
                        projection,
                        spec);

                return new PagedResult<BranchDto>(
                    branches,
                    totalCount);
            });
    }

    public async Task<ResultDto<PagedResult<BranchDto>>> GetOtherBranchesPaginatedAsync(
        OtherBranchesPaginatedModelDto paginatedModelDto)
    {
        return await ExecuteServiceCallAsync(
            "Get Other Branches Paginated",
            async () =>
            {
                var branchSpec = new BaseSpecification<Branch>
                {
                    Includes =
                    [
                        b => b.Merchant
                    ]
                };
                var branch = await _repository
                    .GetAsync(paginatedModelDto.BranchId, branchSpec);
                var merchantId = branch.MerchantId;

                if (merchantId == 0)
                    throw new Exception("Branch not found");

                var spec = new BaseSpecification<Branch>
                {
                    Criteria = b => b.MerchantId == merchantId,
                    Includes =
                    [
                        b => b.Merchant
                    ]
                };

                var paginatedModel = _mapper.Map<PaginatedModel>(paginatedModelDto);

                var (branches, totalCount) =
                    await _repository.GetAllPaginatedAsync(
                        paginatedModel,
                        spec);

                return new PagedResult<BranchDto>(
                    _mapper.Map<IReadOnlyList<BranchDto>>(branches),
                    totalCount);
            });
    }

    private static Point CreateUserLocation(BranchPaginatedModelDto dto)
        => new(dto.Longitude, dto.Latitude) { SRID = 4326 };

    private static Expression<Func<Branch, BranchDto>> ToDto(
        Point userLocation, string baseUrl, int customerId)
    {
        return b => new BranchDto
        {
            Id = b.Id,
            Name = b.Name,
            NameEn = b.NameEn,
            Neighborhood = b.Neighborhood,
            NeighborhoodEn = b.NeighborhoodEn,
            Latitude = b.Location.Y,
            Longitude = b.Location.X,
            DistanceInKm = (decimal)b.Location.Distance(userLocation) / 1000m,
            OpeningTime = b.OpeningTime,
            ClosingTime = b.ClosingTime,
            Status = b.Status,
            LeastPrice = b.BranchesProducts.Any() ? b.BranchesProducts
                .Min(p => p.Product.Price) : 0,
            IsFavorite = b.CustomersBranchesFavorite
                .Any(cbf => cbf.CustomerId == customerId),
            ImagePath = string.IsNullOrEmpty(b.ImagePath)
                ? null
                : baseUrl.TrimEnd('/') + "/" +
                  b.ImagePath.Replace("\\", "/").TrimStart('/'),

            Merchant = new MerchantDto
            {
                Id = b.Merchant.Id,
                Name = b.Merchant.Name
            }
        };
    }
}
