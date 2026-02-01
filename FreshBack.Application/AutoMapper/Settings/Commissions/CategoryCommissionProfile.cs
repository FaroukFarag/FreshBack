using AutoMapper;
using FreshBack.Application.Dtos.Settings.Commissions;
using FreshBack.Domain.Models.Settings.Commissions;

namespace FreshBack.Application.AutoMapper.Settings.Commissions;

public class CategoryCommissionProfile : Profile
{
    public CategoryCommissionProfile()
    {
        CreateMap<CategoryCommission, CategoryCommissionDto>().ReverseMap();

        CreateMap<CategoryCommission, CreateCategoryCommissionDto>().ReverseMap();
    }
}
