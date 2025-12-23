using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Application.Interfaces.Merchants;

public interface IReviewService : IBaseService<
    ReviewDto,
    ReviewDto,
    ReviewDto,
    ReviewDto,
    Review,
    int>
{
}
