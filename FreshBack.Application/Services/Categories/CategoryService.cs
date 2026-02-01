using AutoMapper;
using FreshBack.Application.Dtos.Categories;
using FreshBack.Application.Dtos.Shared;
using FreshBack.Application.Interfaces.Categories;
using FreshBack.Application.Interfaces.Shared;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Constants.Categories;
using FreshBack.Domain.Interfaces.Repositories.Categories;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Models.Categories;

namespace FreshBack.Application.Services.Categories;

public class CategoryService(
    ICategoryRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService) :
    BaseService<
        CreateCategoryDto,
        CategoryDto,
        CategoryDto,
        CategoryDto,
        Category,
        int>(repository, unitOfWork, mapper), ICategoryService
{
    private readonly ICategoryRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IImageService _imageService = imageService;

    public async override Task<ResultDto<CreateCategoryDto>> CreateAsync(
        CreateCategoryDto createCategoryDto)
    {
        return await ExecuteServiceCallAsync(
            "Create Category",
            async () =>
            {
                createCategoryDto.ImagePath =
                    await _imageService.SaveImageAsync(
                        createCategoryDto.ImageFile,
                        CategoryConstants.SubFolder);

                var category = _mapper.Map<Category>(createCategoryDto);

                await _repository.CreateAsync(category);

                if (!await _unitOfWork.Complete())
                    throw new Exception("Failed to create category");

                return _mapper.Map<CreateCategoryDto>(category);
            });
    }
}
