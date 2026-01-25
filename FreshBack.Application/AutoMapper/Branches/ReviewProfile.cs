using AutoMapper;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Application.AutoMapper.Branches;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>().ReverseMap();

        CreateMap<Review, CreateReviewDto>().ReverseMap();
    }
}
