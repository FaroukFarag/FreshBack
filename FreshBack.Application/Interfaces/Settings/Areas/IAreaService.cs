using FreshBack.Application.Dtos.Settings.Areas;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.Settings.Areas;

namespace FreshBack.Application.Interfaces.Settings.Areas;

public interface IAreaService : IBaseService<
    AreaDto,
    AreaDto,
    AreaDto,
    AreaDto,
    Area,
    int>
{
}
