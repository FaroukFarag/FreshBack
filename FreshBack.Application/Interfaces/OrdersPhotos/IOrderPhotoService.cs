using FreshBack.Application.Dtos.OrdersPhotos;
using FreshBack.Application.Interfaces.Abstraction;
using FreshBack.Domain.Models.OrdersPhotos;

namespace FreshBack.Application.Interfaces.OrdersPhotos;

public interface IOrderPhotoService : IBaseService<
    CreateOrderPhotoDto,
    OrderPhotoDto,
    OrderPhotoDto,
    OrderPhotoDto,
    OrderPhoto,
    int>
{
}
