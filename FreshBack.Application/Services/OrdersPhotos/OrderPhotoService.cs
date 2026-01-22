using AutoMapper;
using FreshBack.Application.Dtos.OrdersPhotos;
using FreshBack.Application.Interfaces.OrdersPhotos;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Interfaces.Repositories.OrdersPhotos;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.OrdersPhotos;

namespace FreshBack.Application.Services.OrdersPhotos;

public class OrderPhotoService(
    IOrderPhotoRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : BaseService<
        CreateOrderPhotoDto,
        OrderPhotoDto,
        OrderPhotoDto,
        OrderPhotoDto,
        OrderPhoto,
        int>(repository, unitOfWork, mapper), IOrderPhotoService
{
}
