using AutoMapper;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Application.AutoMapper.Merchants;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>().ReverseMap();
    }
}
