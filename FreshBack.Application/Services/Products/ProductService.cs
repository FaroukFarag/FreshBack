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
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Products;

public class ProductService(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService,
    IProductImageRepository productImageRepository) : BaseService<
    CreateProductDto,
    ProductDto,
    ProductDto,
    CreateProductDto,
    Product,
    int>(repository, unitOfWork, mapper), IProductService
{
    private readonly IProductRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;
    private readonly IProductImageRepository _productImageRepository = productImageRepository;

    public async override Task<ResultDto<CreateProductDto>> CreateAsync(
        CreateProductDto createProductDto)
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

    public async override Task<ResultDto<ProductDto>> GetAsync(int id)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Get Product",
            action: async () =>
            {
                var product = await _repository.GetAsync(id,
                    new BaseSpecification<Product>
                    {
                        Includes =
                        [
                            p => p.ProductImages,
                            p => p.ProductsBranches
                        ]
                    }) ??
                    throw new Exception("Product not found");

                return _mapper.Map<ProductDto>(product);
            });
    }

    public async override Task<ResultDto<CreateProductDto>> UpdateAsync(
        CreateProductDto updateProductDto)
    {
        return await ExecuteServiceCallAsync(
            operationName: "Update Product",
            action: async () =>
            {
                var existingProduct = await _repository.GetAsync(updateProductDto.Id,
                    new BaseSpecification<Product>
                    {
                        Includes =
                        [
                            p => p.ProductImages,
                            p => p.ProductsBranches
                        ]
                    }) ??
                    throw new Exception("Product not found");

                foreach (var productImage in existingProduct.ProductImages)
                {
                    _imageService.DeleteImage(productImage.ImagePath);
                }

                foreach (var productImage in updateProductDto.ProductImages)
                {
                    productImage.ImagePath = await _imageService.SaveImageAsync(productImage.ImageFile, ProductConstants.SubFolder);
                }

                _productImageRepository.DeleteRange(existingProduct.ProductImages);

                _mapper.Map(updateProductDto, existingProduct);

                var updatedProduct = _repository.Update(existingProduct);
                var productUpdated = await _unitOfWork.Complete();

                if (!productUpdated)
                    throw new Exception("Failed to update product");

                return _mapper.Map<CreateProductDto>(updatedProduct);
            });
    }
}
