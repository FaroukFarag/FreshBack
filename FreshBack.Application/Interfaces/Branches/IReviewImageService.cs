using FreshBack.Application.Dtos.Branches;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Application.Interfaces.Branches;

public interface IReviewImageService : IBaseService<
    CreateReviewImageDto,
    ReviewImageDto,
    ReviewImageDto,
    ReviewImageDto,
    ReviewImage,
    int>
{
}
