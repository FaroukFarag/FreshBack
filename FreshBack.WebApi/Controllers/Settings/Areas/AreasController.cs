using FreshBack.Application.Dtos.Settings.Areas;
using FreshBack.Application.Interfaces.Settings.Areas;
using FreshBack.Domain.Models.Settings.Areas;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Settings.Areas;

[Route("api/[controller]")]
[ApiController]
public class AreasController(IAreaService service) : BaseController<IAreaService,
    AreaDto, AreaDto, AreaDto, AreaDto, Area, int>(service)
{
}
