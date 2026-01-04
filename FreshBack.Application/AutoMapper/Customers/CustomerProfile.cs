using AutoMapper;
using FreshBack.Application.Dtos.Customers;
using FreshBack.Domain.Models.Customers;

namespace FreshBack.Application.AutoMapper.Customers;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();

        CreateMap<Customer, CreateCustomerDto>().ReverseMap();
    }
}
