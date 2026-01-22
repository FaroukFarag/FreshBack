using AutoMapper;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Interfaces.Merchants;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Application.Services.Merchants;

public class MerchantService(
    IMerchantRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<CreateMerchantDto, MerchantDto, MerchantDto,
        MerchantDto, Merchant, int>(repository, unitOfWork, mapper), IMerchantService
{
}
