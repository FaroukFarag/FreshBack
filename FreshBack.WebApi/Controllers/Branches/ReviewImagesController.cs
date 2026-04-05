using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Domain.Models.Branches;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Branches;

[Route("api/[controller]")]
[ApiController]
public class ReviewImagesController(IReviewImageService service) : BaseController<IReviewImageService,
    CreateReviewImageDto, ReviewImageDto, ReviewImageDto, ReviewImageDto, ReviewImage, int>(service)
{
    public override Task<IActionResult> Create([FromForm] CreateReviewImageDto createFeedbackDto)
    {
        return base.Create(createFeedbackDto);
    }
}
