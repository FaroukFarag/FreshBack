using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Domain.Models.Branches;
using FreshBack.WebApi.Controllers.Abstraction;
using FreshBack.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Branches;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController(IReviewService service) :
    BaseController<IReviewService, CreateReviewDto, ReviewDto, ReviewDto, ReviewDto,
        Review, int>(service)
{
    private readonly IReviewService _service = service;

    [HttpPost("CreateCustomerReview")]
    public async Task<IActionResult> CreateCustomerReview(
        [FromForm] CreateReviewDto createEntityDto)
    {
        return Ok(await _service.CreateCustomerReviewAsync(
            createEntityDto, User.GetUserId()));
    }

    [HttpPost("Create")]
    public override Task<IActionResult> Create(
        [FromForm] CreateReviewDto createEntityDto)
    {
        return base.Create(createEntityDto);
    }

    [HttpPost("GetReviewsForBranchPaginated")]
    public async Task<IActionResult> Get(
        ReviewsForBranchPaginatedDto reviewsForBranchPaginatedDto)
    {
        return Ok(await _service
            .GetReviewsForBranchPaginatedAsync(reviewsForBranchPaginatedDto));
    }

    [HttpPost("GetReviewsForMerchantPaginated")]
    public async Task<IActionResult> Get(
        ReviewsForMerchantPaginatedDto reviewsForMerchantPaginatedDto)
    {
        return Ok(await _service
            .GetReviewsForMerchantPaginatedAsync(reviewsForMerchantPaginatedDto));
    }
}
