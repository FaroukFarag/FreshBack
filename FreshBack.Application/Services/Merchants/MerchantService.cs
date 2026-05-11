using AutoMapper;
using FreshBack.Application.Dtos.Merchants;
using FreshBack.Application.Dtos.Settings.Users;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Merchants;
using FreshBack.Application.Interfaces.Shared;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Constants.Merchants;
using FreshBack.Domain.Interfaces.Repositories.Merchants;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Roles;
using FreshBack.Domain.Models.Settings.Users;
using FreshBack.Domain.Specifications.Absraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace FreshBack.Application.Services.Merchants;

public class MerchantService(
    IMerchantRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IStringLocalizer<Domain.Resources.Merchants.Merchant> localizer) : BaseService<CreateMerchantDto, MerchantDto, MerchantDto,
        MerchantDto, Merchant, int>(repository, unitOfWork, mapper), IMerchantService
{
    private readonly IMerchantRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly IStringLocalizer<Domain.Resources.Merchants.Merchant> _localizer = localizer;

    public override async Task<ResultDto<CreateMerchantDto>> CreateAsync(
    CreateMerchantDto createMerchantDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Merchant",
            action: async () =>
            {
                createMerchantDto.ImagePath = await _imageService.SaveImageAsync(
                    createMerchantDto.ImageFile,
                    MerchantConstants.SubFolder);

                await using var transaction = await _unitOfWork.BeginTransactionAsync();

                try
                {
                    var merchant = _mapper.Map<Merchant>(createMerchantDto);

                    merchant = await _repository.CreateAsync(merchant);

                    var merchantCreated = await _unitOfWork.Complete();

                    if (!merchantCreated)
                        throw new InvalidOperationException(
                            _localizer["CreationFailed"]);

                    var userDto = _mapper.Map<UserDto>(createMerchantDto);

                    userDto.MerchantId = merchant.Id;

                    var user = _mapper.Map<User>(userDto);

                    var userResult = await _userManager.CreateAsync(user, userDto.Password!);

                    var role = await _roleManager.FindByIdAsync(userDto.RoleId.ToString())
                        ?? throw new InvalidOperationException(_localizer["RoleNotFound"]);

                    if (!userResult.Succeeded)
                        throw new InvalidOperationException(
                            $"{_localizer["UserCreationFailed"]}: {string.Join(",", userResult.Errors.Select(e => e.Description))}");

                    var roleResult = await _userManager.AddToRoleAsync(user, role.Name!);

                    if (!roleResult.Succeeded)
                        throw new InvalidOperationException(
                            _localizer["RoleAssignmentFailed", string.Join(", ", roleResult.Errors.Select(e => e.Description))]);

                    await transaction.CommitAsync();

                    return _mapper.Map<CreateMerchantDto>(merchant);
                }
                catch
                {
                    await transaction.RollbackAsync();

                    // Clean up saved image since DB was rolled back
                    _imageService.DeleteImage(createMerchantDto.ImagePath);

                    throw;
                }
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
                        _localizer["MerchantStatusUpdateFailed"]);

                return _mapper.Map<MerchantDto>(merchant);
            });
    }
}
