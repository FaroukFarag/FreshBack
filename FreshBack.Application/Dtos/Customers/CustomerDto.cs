using FreshBack.Application.Dtos.Abstraction;
using FreshBack.Application.Dtos.Orders;

namespace FreshBack.Application.Dtos.Customers;

public class CustomerDto : BaseModelDto<int>
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public int SavedMeals { get; set; }

    public IEnumerable<OrderDto> Orders { get; set; } = default!;
}
