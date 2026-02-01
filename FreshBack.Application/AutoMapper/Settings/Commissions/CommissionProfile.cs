using AutoMapper;
using FreshBack.Application.Dtos.Settings.Commissions;
using FreshBack.Domain.Models.Settings.Commissions;

namespace FreshBack.Application.AutoMapper.Settings.Commissions;

public class CommissionProfile : Profile
{
    public CommissionProfile()
    {
        CreateMap<Commission, CommissionDto>().ReverseMap();

        CreateMap<Commission, CreateCommissionDto>().ReverseMap();
    }
}
