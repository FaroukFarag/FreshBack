using AutoMapper;
using FreshBack.Application.AutoMapper.Resolvers;
using FreshBack.Application.Dtos.Branches;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Shared;
using NetTopologySuite.Geometries;

namespace FreshBack.Application.AutoMapper.Branches;

public class BranchProfile : Profile
{
    public BranchProfile()
    {
        CreateMap<Branch, BranchDto>()
            .ForMember(d => d.Latitude,
                o => o.MapFrom(s => s.Location.Y))
            .ForMember(d => d.Longitude,
                o => o.MapFrom(s => s.Location.X))
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<BranchDto, Branch>()
            .ForMember(d => d.Location,
                o => o.MapFrom(s =>
                    new Point(s.Longitude, s.Latitude) { SRID = 4326 }))
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());

        CreateMap<BranchPaginatedModelDto, PaginatedModel>();
    }
}
