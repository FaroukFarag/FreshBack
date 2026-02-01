using AutoMapper;
using FreshBack.Application.AutoMapper.Resolvers;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Shared;

namespace FreshBack.Application.AutoMapper.Branches;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<Review, CreateReviewDto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<ReviewDto, Review>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());

        CreateMap<CreateReviewDto, Review>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());

        CreateMap<ReviewsForBranchPaginatedDto, PaginatedModel>();

        CreateMap<ReviewsForMerchantPaginatedDto, PaginatedModel>();
    }
}
