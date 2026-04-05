using AutoMapper;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Dtos.Settings.Users;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Merchants;
using FreshBack.Application.Interfaces.Settings.Users;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Merchants;

namespace FreshBack.Application.Services.Merchants;

public class MerchantService(
    IMerchantRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IUserService userService) : BaseService<CreateMerchantDto, MerchantDto, MerchantDto,
        MerchantDto, Merchant, int>(repository, unitOfWork, mapper), IMerchantService
{
    private readonly IMerchantRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IUserService _userService = userService;

    public override async Task<ResultDto<CreateMerchantDto>> CreateAsync(
        CreateMerchantDto createMerchantDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Merchant",
            action: async () =>
            {
                var merchant = _mapper.Map<Merchant>(createMerchantDto);

                merchant = await _repository.CreateAsync(merchant);

                var merchantCreated = await _unitOfWork.Complete();

                if (!merchantCreated)
                    throw new InvalidOperationException(
                        $"An error occurred while creating the merchant");

                var userDto = _mapper.Map<UserDto>(createMerchantDto);

                userDto.MerchantId = merchant.Id;

                var userResult = await _userService.CreateAsync(userDto);

                if (!userResult.Succeeded)
                    throw new InvalidOperationException(
                        $"Failed to assign user the merchant");

                return _mapper.Map<CreateMerchantDto>(merchant);
            });
    }
}
