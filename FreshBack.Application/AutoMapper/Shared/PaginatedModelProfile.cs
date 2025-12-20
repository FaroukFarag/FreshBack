using AutoMapper;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Domain.Models.Shared;

namespace FreshBack.Application.AutoMapper.Shared;

public class PaginatedModelProfile : Profile
{
    public PaginatedModelProfile()
    {
        CreateMap<PaginatedModel, PaginatedModelDto>().ReverseMap();
    }
}
