using FreshBack.Application.Dtos.Customers;
using FreshBack.Application.Interfaces.Customers;
using FreshBack.Domain.Models.Customers;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Customers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(ICustomerService service) :
    BaseController<ICustomerService, CreateCustomerDto, CustomerDto, CustomerDto,
        CustomerDto, Customer, int>(service)
{
}
