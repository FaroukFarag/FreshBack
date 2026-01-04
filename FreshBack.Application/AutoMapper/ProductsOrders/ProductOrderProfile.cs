using AutoMapper;
using FreshBack.Application.Dtos.ProductsOrders;
using FreshBack.Domain.Models.ProductsOrders;

namespace FreshBack.Application.AutoMapper.ProductsOrders;

public class ProductOrderProfile : Profile
{
    public ProductOrderProfile()
    {
        CreateMap<ProductOrder, ProductOrderDto>().ReverseMap();
    }
}
