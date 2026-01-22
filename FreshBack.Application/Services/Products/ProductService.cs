using AutoMapper;
using FreshBack.Application.Dtos.Products;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Products;
using FreshBack.Application.Interfaces.Shared;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Constants.Products;
using FreshBack.Domain.Interfaces.Repositories.Products;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Products;

namespace FreshBack.Application.Services.Products;

public class ProductService(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService) : BaseService<
    CreateProductDto,
    ProductDto,
    ProductDto,
    ProductDto,
    Product,
    int>(repository, unitOfWork, mapper), IProductService
{
    private readonly IProductRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;

    public async override Task<ResultDto<CreateProductDto>> CreateAsync(CreateProductDto createProductDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Create Product",
            action: async () =>
            {
                foreach (var productImage in createProductDto.ProductImages)
                {
                    productImage.ImagePath = await _imageService.SaveImageAsync(productImage.ImageFile, ProductConstants.SubFolder);
                }

                var product = _mapper.Map<Product>(createProductDto);

                product = await _repository.CreateAsync(product);

                var productCreated = await _unitOfWork.Complete();

                if (!productCreated)
                    throw new Exception("Failed to create product");

                return _mapper.Map<CreateProductDto>(product);
            });
    }
}
