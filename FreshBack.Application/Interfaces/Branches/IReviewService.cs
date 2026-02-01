using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Application.Interfaces.Branches;

public interface IReviewService : IBaseService<
    CreateReviewDto,
    ReviewDto,
    ReviewDto,
    ReviewDto,
    Review,
    int>
{
    Task<ResultDto<CreateReviewDto>> CreateCustomerReviewAsync(
        CreateReviewDto createReviewDto, int customerId);

    Task<ResultDto<PagedResult<ReviewDto>>> GetReviewsForBranchPaginatedAsync(
        ReviewsForBranchPaginatedDto reviewsForBranchPaginatedDto);

    Task<ResultDto<PagedResult<ReviewDto>>> GetReviewsForMerchantPaginatedAsync(
        ReviewsForMerchantPaginatedDto reviewsForMerchantPaginated);
}
