using FreshBack.Application.Dtos.Settings.Commissions;
using FreshBack.Application.Interfaces.Settings.Commissions;
using FreshBack.Domain.Models.Settings.Commissions;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.Settings.Commissions;

[Route("api/[controller]")]
[ApiController]
public class CommissionsController(ICommissionService service) : BaseController<
    ICommissionService, CreateCommissionDto, CommissionDto, CommissionDto,
    CreateCommissionDto, Commission, int>(service)
{
}
