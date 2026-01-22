using FreshBack.Application.Dtos.Shared;

namespace FreshBack.Application.Dtos.Addresses;

public class AddressPaginatedModelDto : PaginatedModelDto
{
    public int CustomerId { get; set; }
}
