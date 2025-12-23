using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Application.Interfaces.Merchants;

public interface IMerchantService : IBaseService<
    MerchantDto,
    MerchantDto,
    MerchantDto,
    MerchantDto,
    Merchant,
    int>
{
}
