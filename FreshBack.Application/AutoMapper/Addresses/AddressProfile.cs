using AutoMapper;
using FreshBack.Application.Dtos.Addresses;
using FreshBack.Domain.Models.Addresses;
using FreshBack.Domain.Models.Shared;

namespace FreshBack.Application.AutoMapper.Addresses;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();

        CreateMap<Address, CreateAddressDto>().ReverseMap();

        CreateMap<AddressPaginatedModelDto, PaginatedModel>();
    }
}
