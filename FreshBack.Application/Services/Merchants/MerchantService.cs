using AutoMapper;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Dtos.Settings.Users;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Merchants;
using FreshBack.Application.Interfaces.Settings.Users;
using FreshBack.Application.Interfaces.Shared;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Constants.Merchants;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Merchants;

public class MerchantService(
    IMerchantRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService,
    IUserService userService) : BaseService<CreateMerchantDto, MerchantDto, MerchantDto,
        MerchantDto, Merchant, int>(repository, unitOfWork, mapper), IMerchantService
{
    private readonly IMerchantRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly IUserService _userService = userService;

    public override async Task<ResultDto<CreateMerchantDto>> CreateAsync(
        CreateMerchantDto createMerchantDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Merchant",
            action: async () =>
            {
                createMerchantDto.ImagePath =
                    await _imageService.SaveImageAsync(
                        createMerchantDto.ImageFile,
                        MerchantConstants.SubFolder);

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

    public override async Task<ResultDto<MerchantDto>> GetAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            "Get Merchant",
            async () =>
            {
                var merchant = await _repository.GetAsync(id, new BaseSpecification<Merchant>
                {
                    IncludeChains =
                    [
                        new()
                        {
                            InitialInclude = m => m.Reviews,
                            ThenIncludes =
                            [
                                r => (r as Review)!.ReviewImages
                            ]
                        }
                    ]
                });

                return _mapper.Map<MerchantDto>(merchant);
            });
    }

    public async Task<ResultDto<MerchantDto>> UpdateStatus(UpdateStatusDto updateStatusDto)
    {
        return await ExecuteServiceCallAsync(
            "Update Merchant Status",
            async () =>
            {
                var merchant = await _repository.GetAsync(updateStatusDto.Id);

                merchant.Status = updateStatusDto.Status;

                merchant = _repository.Update(merchant);

                var merchantUpdated = await _unitOfWork.Complete();

                if (!merchantUpdated)
                    throw new InvalidOperationException(
                        $"An error occurred while updating the merchant status");

                return _mapper.Map<MerchantDto>(merchant);
            });
    }
}
