using AutoMapper;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Application.Interfaces.Shared;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Constants.Branches;
using FreshBack.Domain.Interfaces.Repositories.Branches;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Branches;

public class ReviewService(
    IReviewRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService) : BaseService<CreateReviewDto, ReviewDto, ReviewDto, ReviewDto,
        Review, int>(repository, unitOfWork, mapper), IReviewService
{
    private readonly IReviewRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;

    public async Task<ResultDto<CreateReviewDto>> CreateCustomerReviewAsync(
        CreateReviewDto createReviewDto, int customerId)
    {
        foreach (var reviewImage in createReviewDto.ReviewImages)
        {
            reviewImage.ImagePath = await _imageService
                .SaveImageAsync(reviewImage.ImageFile, ReviewConstants.SubFolder);
        }

        createReviewDto.CustomerId = customerId;

        return await base.CreateAsync(createReviewDto);
    }

    public async Task<ResultDto<PagedResult<ReviewDto>>>
        GetReviewsForBranchPaginatedAsync(
            ReviewsForBranchPaginatedDto reviewsForBranchPaginatedDto)
    {
        return await ExecuteServiceCallAsync(
            "Get Reviews For Branch Paginated",
            async () =>
            {
                var paginatedModel = _mapper
                    .Map<PaginatedModel>(reviewsForBranchPaginatedDto);
                var spec = new BaseSpecification<Review>
                {
                    Criteria = r => r.BranchId == reviewsForBranchPaginatedDto.BranchId
                };
                var (reviews, totalCount) = await _repository
                    .GetAllPaginatedAsync(paginatedModel, spec);

                return new PagedResult<ReviewDto>(
                    _mapper.Map<IReadOnlyList<ReviewDto>>(reviews), totalCount);
            });
    }

    public async Task<ResultDto<PagedResult<ReviewDto>>>
        GetReviewsForMerchantPaginatedAsync(
            ReviewsForMerchantPaginatedDto reviewsForBranchPaginatedDto)
    {
        return await ExecuteServiceCallAsync(
            "Get Reviews For Merchant Paginated",
            async () =>
            {
                var paginatedModel = _mapper
                    .Map<PaginatedModel>(reviewsForBranchPaginatedDto);
                var spec = new BaseSpecification<Review>
                {
                    Criteria = r => r.Branch.MerchantId ==
                        reviewsForBranchPaginatedDto.MerchantId
                };
                var (reviews, totalCount) = await _repository
                    .GetAllPaginatedAsync(paginatedModel, spec);

                return new PagedResult<ReviewDto>(
                    _mapper.Map<IReadOnlyList<ReviewDto>>(reviews), totalCount);
            });
    }
}
