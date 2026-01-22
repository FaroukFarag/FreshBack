using AutoMapper;
using FreshBack.Application.AutoMapper.Resolvers;
using FreshBack.Application.Dtos.OrdersPhotos;
using FreshBack.Domain.Models.OrdersPhotos;

namespace FreshBack.Application.AutoMapper.OrdersPhotos;

public class OrderPhotoProfile : Profile
{
    public OrderPhotoProfile()
    {
        CreateMap<OrderPhoto, OrderPhotoDto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<OrderPhoto, CreateOrderPhotoDto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageDtoUrlResolver>());

        CreateMap<OrderPhotoDto, OrderPhoto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());

        CreateMap<CreateOrderPhotoDto, OrderPhoto>()
            .ForMember(des => des.ImagePath, opt => opt
                .MapFrom<BaseModelImageUrlResolver>());
    }
}
