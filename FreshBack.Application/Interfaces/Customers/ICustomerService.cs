using FreshBack.Application.Dtos.Customers;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Customers;

namespace FreshBack.Application.Interfaces.Customers;

public interface ICustomerService : IBaseService<
    CreateCustomerDto,
    CustomerDto,
    CustomerDto,
    CustomerDto,
    Customer,
    int>
{
    Task<ResultDto<CreateCustomerDto>> LoginAsync(LoginCustomerDto loginCustomerDto);
}
