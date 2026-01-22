using FreshBack.Application.Dtos.OtpCodes;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.OtpCodes;

namespace FreshBack.Application.Interfaces.OtpCodes;

public interface IOtpCodeService : IBaseService<
    OtpCodeDto,
    OtpCodeDto,
    OtpCodeDto,
    OtpCodeDto,
    OtpCode,
    int>
{
    Task<ResultDto<OtpVerifiedDto>> VerifyOtp(OtpCodeDto otpCodeDto);
}
