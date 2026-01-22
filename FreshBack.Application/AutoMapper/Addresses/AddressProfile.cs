using AutoMapper;
using FreshBack.Application.Dtos.Addresses;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Domain.Models.Addresses;

namespace FreshBack.Application.AutoMapper.Addresses;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Address, AddressDto>().ReverseMap();

        CreateMap<Address, CreateAddressDto>().ReverseMap();

        CreateMap<PaginatedModelDto, AddressPaginatedModelDto>();
    }
}
