using AutoMapper;
using FreshBack.Application.AutoMapper.Resolvers;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Application.AutoMapper.Branches;

public class ReviewImageProfile : Profile
{
    public ReviewImageProfile()
    {
        CreateMap<ReviewImage, CreateReviewImageDto>()
            .ForMember(d => d.ImagePath, opt => opt
                .MapFrom<ImagePathToUrlResolver<ReviewImage, CreateReviewImageDto>>());

        CreateMap<CreateReviewImageDto, ReviewImage>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<CreateReviewImageDto, ReviewImage>>());

        CreateMap<ReviewImage, ReviewImageDto>()
            .ForMember(d => d.ImagePath, opt => opt
                .MapFrom<ImagePathToUrlResolver<ReviewImage, ReviewImageDto>>());

        CreateMap<ReviewImageDto, ReviewImage>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<ReviewImageDto, ReviewImage>>());
    }
}
