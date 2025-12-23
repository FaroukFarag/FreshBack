using AutoMapper;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Interfaces.Merchants;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Application.Services.Merchants;

public class ReviewService(
    IReviewRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<ReviewDto, ReviewDto, ReviewDto, ReviewDto,
        Review, int>(repository, unitOfWork, mapper), IReviewService
{
}
