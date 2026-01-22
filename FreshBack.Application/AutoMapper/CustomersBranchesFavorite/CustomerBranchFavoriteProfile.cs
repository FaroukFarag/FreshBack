using AutoMapper;
using FreshBack.Application.Dtos.CustomersBranchesFavorite;
using FreshBack.Domain.Models.BranchesFavorites;

namespace FreshBack.Application.AutoMapper.CustomersBranchesFavorite;

public class CustomerBranchFavoriteProfile : Profile
{
    public CustomerBranchFavoriteProfile()
    {
        CreateMap<CustomerBranchFavorite, CustomerBranchFavoriteDto>().ReverseMap();

        CreateMap<CustomerBranchFavorite, CreateCustomerBranchFavoriteDto>()
            .ReverseMap();
    }
}
