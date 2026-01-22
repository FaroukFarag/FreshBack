using FreshBack.Application.Dtos.Addresses;
using FreshBack.Application.Interfaces.Addresses;
using FreshBack.Domain.Models.Addresses;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Addresses;

[Route("api/[controller]")]
[ApiController]
public class AddressesController(IAddressService service) :
    BaseController<IAddressService, CreateAddressDto, AddressDto, AddressDto,
        AddressDto, Address, int>(service)
{
    private readonly IAddressService _service = service;

    [HttpPost]
    public async Task<IActionResult> GetAddressesForCustomer(
        [FromBody] AddressPaginatedModelDto addressPaginatedModelDto)
    {
        return Ok(await _service.GetAddressesForCustomerAsync(addressPaginatedModelDto));
    }
}
