using AutoMapper;
using FreshBack.Application.Dtos.Orders;
using FreshBack.Domain.Models.Orders;

namespace FreshBack.Application.AutoMapper.Orders;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
    }
}
