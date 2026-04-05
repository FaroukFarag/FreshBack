using AutoMapper;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Shared;

namespace FreshBack.Application.AutoMapper.Branches;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>().ReverseMap();

        CreateMap<Review, CreateReviewDto>().ReverseMap();

        CreateMap<ReviewsForBranchPaginatedDto, PaginatedModel>();

        CreateMap<ReviewsForMerchantPaginatedDto, PaginatedModel>();
    }
}
