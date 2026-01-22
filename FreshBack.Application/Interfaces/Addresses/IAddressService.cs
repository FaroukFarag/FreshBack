using FreshBack.Application.Dtos.Addresses;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Addresses;

namespace FreshBack.Application.Interfaces.Addresses;

public interface IAddressService : IBaseService<
    CreateAddressDto,
    AddressDto,
    AddressDto,
    AddressDto,
    Address,
    int>
{
    Task<ResultDto<PagedResult<AddressDto>>> GetAddressesForCustomerAsync(
        AddressPaginatedModelDto addressPaginatedModelDto);
}
