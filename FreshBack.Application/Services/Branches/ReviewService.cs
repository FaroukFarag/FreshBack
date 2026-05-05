using AutoMapper;
using FreshBack.Application.Configurations;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Helpers;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Application.Interfaces.Shared;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Constants.Branches;
using FreshBack.Domain.Interfaces.Repositories.Branches;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;
using Microsoft.Extensions.Options;

namespace FreshBack.Application.Services.Branches;

public class ReviewService(
    IReviewRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService,
    IMerchantRepository merchantRepository,
    IOptions<ImageSettings> settings) : BaseService<CreateReviewDto, ReviewDto, ReviewDto,
        ReviewDto, Review, int>(repository, unitOfWork, mapper), IReviewService
{
    private readonly IReviewRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly IMerchantRepository _merchantRepository = merchantRepository;
    private readonly ImageSettings _settings = settings.Value;

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

    public override async Task<ResultDto<ReviewDto>> GetAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            "Get Reviews For Branch Paginated",
            async () =>
            {
                var spec = new BaseSpecification<Review>
                {
                    Includes =
                    [
                        r => r.ReviewImages
                    ]
                };

                return _mapper.Map<ReviewDto>(await _repository.GetAsync(id, spec));
            });
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

    public async Task<ResultDto<PagedResult<MerchantReviewDto>>>
        GetAllMerchantsReviewsPaginatedAsync(
            PaginatedModelDto paginatedModelDto)
    {
        return await ExecuteServiceCallAsync(
            "Get Reviews For Merchant Paginated",
            async () =>
            {
                var paginatedModel = _mapper
                    .Map<PaginatedModel>(paginatedModelDto);
                var spec = new BaseSpecification<Merchant>
                {
                    Criteria = m => m.Reviews.Any()
                };
                var (merchantsReviews, totalCount) = await _merchantRepository
                    .GetAllPaginatedAsync(
                        paginatedModel,
                        m => new MerchantReviewDto
                        {
                            Id = m.Id,
                            ReviewId = m.Reviews.FirstOrDefault()!.Id,
                            ImagePath = ImagePathHelper.ToFullUrl(m.Reviews.FirstOrDefault()!.ReviewImages.FirstOrDefault()!.ImagePath, _settings.BaseUrl)
                        },
                        spec);

                return new PagedResult<MerchantReviewDto>(merchantsReviews, totalCount);
            });
    }
}
