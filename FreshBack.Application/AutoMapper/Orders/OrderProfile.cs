using AutoMapper;
using FreshBack.Application.Dtos.Orders;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.Shared;

namespace FreshBack.Application.AutoMapper.Orders;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();

        CreateMap<Order, CreateOrderDto>().ReverseMap();

        CreateMap<GetCustomerPreviousOrdersDto, PaginatedModel>();
    }
}
