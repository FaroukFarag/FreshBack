using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Customers;

namespace FreshBack.Application.Dtos.Carts;

public class CartDto : BaseModelDto<int>
{
    public int CustomerId { get; set; }

    public CustomerDto? Customer { get; set; }
}
