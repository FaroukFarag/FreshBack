using AutoMapper;
using FreshBack.Application.Dtos.Addresses;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Addresses;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Addresses;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Addresses;
using FreshBack.Domain.Models.Shared;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Addresses;

public class AddressService(
    IAddressRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<
    CreateAddressDto,
    AddressDto,
    AddressDto,
    AddressDto,
    Address,
    int>(repository, unitOfWork, mapper), IAddressService
{
    private readonly IAddressRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResultDto<PagedResult<AddressDto>>> GetAddressesForCustomerAsync(
        AddressPaginatedModelDto paginatedModelDto)
    {
        return await ExecuteServiceCallAsync(
            "Get Addresses For Customer Paginated",
            async () =>
            {
                var spec = new BaseSpecification<Address>
                {
                    Criteria = a => a.CustomerId == paginatedModelDto.CustomerId
                };
                var paginatedModel =
                    _mapper.Map<PaginatedModel>(paginatedModelDto);

                var (addresses, totalCount) =
                    await _repository.GetAllPaginatedAsync(
                        paginatedModel,
                        spec);

                return new PagedResult<AddressDto>(
                    _mapper.Map<IReadOnlyList<AddressDto>>(addresses),
                    totalCount);
            });
    }
}
