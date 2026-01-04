using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Interfaces.Orders;
using FreshBack.Domain.Models.Orders;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Orders;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IOrderService service) :
    BaseController<IOrderService, CreateOrderDto, OrderDto, OrderDto, OrderDto, Order,
    int>(service)
{
}
