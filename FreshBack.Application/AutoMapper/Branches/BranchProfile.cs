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
                o => o.MapFrom(s => s.Location != null ? s.Location.Y : 0))
            .ForMember(d => d.Longitude,
                o => o.MapFrom(s => s.Location != null ? s.Location.X : 0))
            .ForMember(d => d.ImagePath,
                opt => opt.MapFrom<ImagePathToUrlResolver<Branch, BranchDto>>())
            .ForMember(d => d.TotalReviews,
                opt => opt.MapFrom(src =>
                    src.Reviews != null && src.Reviews.Any()
                        ? src.Reviews.Average(r => r.Rating)
                        : 0));


        CreateMap<BranchDto, Branch>()
            .ForMember(d => d.Location,
                o => o.MapFrom(s =>
                    new Point(s.Longitude, s.Latitude) { SRID = 4326 }))
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<BranchDto, Branch>>());

        CreateMap<CreateBranchDto, Branch>()
            .ForMember(d => d.Location,
                o => o.MapFrom(s =>
                    new Point(s.Longitude, s.Latitude) { SRID = 4326 }))
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImageUrlToPathResolver<CreateBranchDto, Branch>>());

        CreateMap<Branch, CreateBranchDto>()
            .ForMember(d => d.Latitude,
                o => o.MapFrom(s => s.Location.Y))
            .ForMember(d => d.Longitude,
                o => o.MapFrom(s => s.Location.X))
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<ImagePathToUrlResolver<Branch, CreateBranchDto>>());

        CreateMap<BranchPaginatedModelDto, PaginatedModel>();

        CreateMap<OtherBranchesPaginatedModelDto, PaginatedModel>();
    }
}
