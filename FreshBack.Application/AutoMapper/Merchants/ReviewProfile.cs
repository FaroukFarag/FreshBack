using AutoMapper;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Domain.Models.Branches;

namespace FreshBack.Application.AutoMapper.Merchants;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>().ReverseMap();
    }
}
