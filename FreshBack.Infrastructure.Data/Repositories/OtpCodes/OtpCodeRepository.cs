using FreshBack.Domain.Interfaces.Repositories.OtpCodes;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.OtpCodes;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.OtpCodes;

public class OtpCodeRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<OtpCode> specificationCombiner) :
    BaseRepository<OtpCode, int>(context, specificationCombiner), IOtpCodeRepository
{
}
