using FreshBack.Application.Dtos.Customers;
using FreshBack.Application.Interfaces.Customers;
using FreshBack.Domain.Models.Customers;
using FreshBack.WebApi.Controllers.Abstraction;
using FreshBack.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Customers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(ICustomerService service) :
    BaseController<ICustomerService, CreateCustomerDto, CustomerDto, CustomerDto,
        UpdateCustomerDto, Customer, int>(service)
{
    private readonly ICustomerService _service = service;

    [HttpGet("GetCustomer")]
    public Task<IActionResult> Get()
    {
        return base.Get(User.GetUserId());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginCustomerDto loginCustomerDto)
    {
        return Ok(await _service.LoginAsync(loginCustomerDto));
    }
}
