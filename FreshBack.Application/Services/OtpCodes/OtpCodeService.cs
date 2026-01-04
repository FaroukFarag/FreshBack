using AutoMapper;
using FreshBack.Application.Dtos.OtpCodes;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.OtpCodes;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.OtpCodes;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.OtpCodes;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.OtpCodes;

public class OtpCodeService(
    IOtpCodeRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<
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

    public async Task<ResultDto<OtpCodeDto>> VerifyOtp(OtpCodeDto otpCodeDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Verify Otp",
            action: async () =>
            {
                var spec = new BaseSpecification<OtpCode>
                {
                    Criteria = otp => otp.PhoneNumber == otpCodeDto.PhoneNumber && !otp.IsUsed,
                    OrderByDescending = otp => otp.ExpireAt
                };

                var otpRecord = (await _repository.GetAllAsync(spec))
                    .FirstOrDefault();

                if (otpRecord == null ||
                    otpRecord.ExpireAt < DateTime.UtcNow ||
                    !BCrypt.Net.BCrypt.Verify(otpCodeDto.Otp, otpRecord.CodeHash))
                {
                    throw new Exception("Invalid or expired OTP");
                }

                otpRecord.IsUsed = true;

                return _mapper.Map<OtpCodeDto>(otpRecord);
            });
    }
}
