using AutoMapper;
using FreshBack.Application.Dtos.Settings.Areas;
using FreshBack.Application.Interfaces.Settings.Areas;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.Settings.Areas;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Settings.Areas;

namespace FreshBack.Application.Services.Settings.Areas;

public class AreaService(
    IAreaRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<AreaDto, AreaDto, AreaDto, AreaDto,
        Area, int>(repository, unitOfWork, mapper),
    IAreaService
{
}
