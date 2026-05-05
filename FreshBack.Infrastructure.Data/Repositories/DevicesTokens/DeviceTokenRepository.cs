using FreshBack.Domain.Interfaces.Repositories.DevicesTokens;
using FreshBack.Domain.Interfaces.Specifications.Absraction;
using FreshBack.Domain.Models.DevicesTokens;
using FreshBack.Infrastructure.Data.Context;
using FreshBack.Infrastructure.Data.Repositories.Abstraction;

namespace FreshBack.Infrastructure.Data.Repositories.DevicesTokens;

public class DeviceTokenRepository(
    FreshBackDbContext context,
    ISpecificationCombiner<DeviceToken> specificationCombiner) :
    BaseRepository<DeviceToken, int>(context, specificationCombiner), IDeviceTokenRepository
{
}
