using AutoMapper;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Interfaces.Branches;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Branches;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Application.Services.Branches;

public class ReviewService(
    IReviewRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<CreateReviewDto, ReviewDto, ReviewDto, ReviewDto,
        Review, int>(repository, unitOfWork, mapper), IReviewService
{
}
