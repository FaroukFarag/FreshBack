using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Domain.Models.Branches;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Branches;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController(IReviewService service) :
    BaseController<IReviewService, CreateReviewDto, ReviewDto, ReviewDto, ReviewDto,
        Review, int>(service)
{
}
