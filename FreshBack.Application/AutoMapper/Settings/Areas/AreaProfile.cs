using AutoMapper;
using FreshBack.Application.Dtos.Settings.Areas;
using FreshBack.Domain.Models.Settings.Areas;

namespace FreshBack.Application.AutoMapper.Settings.Areas;

public class AreaProfile : Profile
{
    public AreaProfile()
    {
        CreateMap<Area, AreaDto>();
    }
}
