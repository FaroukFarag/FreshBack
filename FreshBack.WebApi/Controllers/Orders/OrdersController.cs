using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Interfaces.Orders;
using FreshBack.Domain.Models.Orders;
using FreshBack.WebApi.Controllers.Abstraction;
using FreshBack.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Orders;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IOrderService service) :
    BaseController<IOrderService, CreateOrderDto, OrderDto, OrderDto, OrderDto, Order,
    int>(service)
{
    private readonly IOrderService _service = service;

    [HttpPost("CreateCustomeOrder")]
    public async Task<IActionResult> CreateCustomeOrder(
        CreateOrderDto createOrderDto)
    {
        return Ok(await _service.CreateAsync(createOrderDto, User.GetUserId()));
    }

    [HttpPost("GetCustomerOrders")]
    public async Task<IActionResult> GetCustomerOrders(
        GetCustomerPreviousOrdersDto getCustomerPreviousOrdersDto)
    {
        return Ok(await _service.GetCustomerOrders(
            getCustomerPreviousOrdersDto,
            User.GetUserId()));
    }
}
