using AutoMapper;
using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Domain.Models.Abstraction;

namespace FreshBack.Application.AutoMapper.Abstraction;

public class BaseModelProfile : Profile
{
    public BaseModelProfile()
    {
        CreateMap(typeof(BaseModel<>), typeof(BaseModelDto<>)).ReverseMap();
    }
}
