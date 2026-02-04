using AutoMapper;
using FreshBack.Application.Dtos.BranchesProducts;
using FreshBack.Domain.Models.BranchesProducts;

namespace FreshBack.Application.AutoMapper.BranchesProducts;

public class BranchProductProfile : Profile
{
    public BranchProductProfile()
    {
        CreateMap<BranchProduct, BranchProductDto>().ReverseMap();

        CreateMap<BranchProduct, CreateBranchProductDto>().ReverseMap();
    }
}
