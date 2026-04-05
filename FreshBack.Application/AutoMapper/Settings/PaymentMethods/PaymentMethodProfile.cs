using AutoMapper;
using FreshBack.Application.Dtos.Settings.PaymentMethods;
using FreshBack.Domain.Models.Settings.PaymentMethods;

namespace FreshBack.Application.AutoMapper.Settings.PaymentMethods;

public class PaymentMethodProfile : Profile
{
    public PaymentMethodProfile()
    {
        CreateMap<PaymentMethod, PaymentMethodDto>().ReverseMap();
    }
}
