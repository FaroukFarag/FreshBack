using FreshBack.Application.Dtos.OrdersPhotos;
using FreshBack.Application.Interfaces.OrdersPhotos;
using FreshBack.Domain.Models.OrdersPhotos;
using FreshBack.WebApi.Controllers.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FreshBack.WebApi.Controllers.OrdersPhotos;

[Route("api/[controller]")]
[ApiController]
public class OrdersPhotosController(IOrderPhotoService service) :
    BaseController<IOrderPhotoService, CreateOrderPhotoDto, OrderPhotoDto,
        OrderPhotoDto, OrderPhotoDto, OrderPhoto, int>(service)
{
}
