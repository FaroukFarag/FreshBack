using AutoMapper;
using FreshBack.Application.Dtos.Orders;
using FreshBack.Application.Interfaces.Orders;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Orders;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Application.Services.Orders;

public class OrderService(
    IOrderRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<
    OrderDto,
    OrderDto,
    OrderDto,
    OrderDto,
    Order,
    int>(repository, unitOfWork, mapper), IOrderService
{
}
