using AutoMapper;
using FreshBack.Application.Dtos.Customers;
using FreshBack.Application.Dtos.OtpCodes;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.OtpCodes;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Common.Tokens.Interfaces;
using FreshBack.Domain.Enums.Roles;
using FreshBack.Domain.Interfaces.Repositories.Customers;
using FreshBack.Domain.Interfaces.Repositories.OtpCodes;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.OtpCodes;
using FreshBack.Domain.Specifications.Absraction;
using System.Security.Claims;

namespace FreshBack.Application.Services.OtpCodes;

public class OtpCodeService(
    IOtpCodeRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICustomerRepository customerRepository,
    ITokensService tokensService) : BaseService<
    OtpCodeDto,
    OtpCodeDto,
    OtpCodeDto,
    OtpCodeDto,
    OtpCode,
    int>(repository, unitOfWork, mapper), IOtpCodeService
{
    private readonly IOtpCodeRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ITokensService _tokensService = tokensService;

    public async override Task<ResultDto<OtpCodeDto>> CreateAsync(OtpCodeDto otpCodeDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Otp Code",
            action: async () =>
            {
                var otp = new Random().Next(100000, 999999).ToString();

                var otpEntity = new OtpCode
                {
                    PhoneNumber = otpCodeDto.PhoneNumber,
                    CodeHash = BCrypt.Net.BCrypt.HashPassword(otp),
                    ExpireAt = DateTime.UtcNow.AddMinutes(5),
                    IsUsed = false
                };
                var otpCode = await _repository.CreateAsync(otpEntity);
                var savedSuccessfully = await _unitOfWork.Complete();

                if (!savedSuccessfully)
                    throw new Exception($"Failed to create Otp Code");

                otpCodeDto.Otp = otp;

                return otpCodeDto;
            });
    }

    public async Task<ResultDto<OtpVerifiedDto>> VerifyOtp(OtpCodeDto otpCodeDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Verify Otp",
            action: async () =>
            {
                if (otpCodeDto.Otp is null)
                    throw new Exception("Invalid Otp");
                var otpRecord = await GetLatestValidOtp(otpCodeDto.PhoneNumber, otpCodeDto.Otp);

                otpRecord.IsUsed = true;

                var customer = await GetCustomer(otpCodeDto.PhoneNumber);

                var claims = BuildCustomerClaims(customer.Id);

                return new OtpVerifiedDto
                {
                    Token = await _tokensService.GenerateToken(claims),
                    Customer = _mapper.Map<CustomerDto>(customer)
                };
            });
    }

    private async Task<OtpCode> GetLatestValidOtp(string phoneNumber, string otp)
    {
        var spec = new BaseSpecification<OtpCode>
        {
            Criteria = o => o.PhoneNumber == phoneNumber && !o.IsUsed,
            OrderByDescending = o => o.ExpireAt
        };

        var otpRecord = (await _repository.GetAllAsync(spec)).FirstOrDefault();

        if (otpRecord == null ||
            otpRecord.ExpireAt < DateTime.UtcNow ||
            !BCrypt.Net.BCrypt.Verify(otp, otpRecord.CodeHash))
        {
            throw new Exception("Invalid or expired OTP");
        }

        return otpRecord;
    }

    private async Task<Customer> GetCustomer(string phoneNumber)
    {
        var spec = new BaseSpecification<Customer>
        {
            Criteria = c => c.PhoneNumber == phoneNumber
        };
        var customer = (await _customerRepository.GetAllAsync(spec)).FirstOrDefault();

        if (customer != null)
            return customer;

        customer = await _customerRepository.CreateAsync(new Customer
        {
            PhoneNumber = phoneNumber
        });
        var customerCreated = await _unitOfWork.Complete();

        if (!customerCreated)
            throw new Exception("Failed to create customer");

        return customer;
    }

    private static List<TokenClaim> BuildCustomerClaims(int customerId)
    {
        return new List<TokenClaim>
    {
        new(ClaimTypes.NameIdentifier, customerId.ToString()),
        new(ClaimTypes.Role, RoleNames.Customer.ToString())
    };
    }

}
