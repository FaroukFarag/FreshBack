using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.OtpCodes;

namespace FreshBack.Domain.Interfaces.Repositories.OtpCodes;

public interface IOtpCodeRepository : IBaseRepository<OtpCode, int>
{
}
