using AutoMapper;
using FreshBack.Application.Dtos.Carts;
using FreshBack.Application.Interfaces.Carts;
using FreshBack.Application.Services.Abstraction;
using FreshBack.Domain.Carts;
using FreshBack.Domain.Interfaces.Repositories.Carts;
using FreshBack.Domain.Interfaces.Repositories.Products;
using FreshBack.Domain.Interfaces.UnitOfWork;
using FreshBack.Domain.Specifications.Absraction;

namespace FreshBack.Application.Services.Carts;

public class CartService(
    ICartRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IProductRepository productRepository) :
    BaseService<
        CartDto,
        CartDto,
        CartDto,
        CartDto,
        Cart,
        int>(repository, unitOfWork, mapper), ICartService
{
    private readonly ICartRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<CartDto> SyncCartAsync(int customerId, List<CartItemDto> cartItems)
    {
        var spec = new BaseSpecification<Cart>
        {
            Criteria = c => c.CustomerId == customerId,
            Includes =
            [
                c => c.CartItems
            ]
        };
        var carts = await _repository.GetAllAsync(spec);
        var cart = carts.FirstOrDefault() ?? new Cart { CustomerId = customerId };

        foreach (var item in cartItems)
        {
            var product = await _productRepository.GetAsync(item.ProductId);

            if (product == null) continue;

            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            var quantity = Math.Min(item.Quantity, product.Quantity);

            if (cartItem == null)
            {
                cart.CartItems.Append(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                    Price = product.Price
                });
            }

            else
            {
                cartItem.Quantity = Math.Min(
                    cartItem.Quantity + quantity,
                    product.Quantity);
            }
        }

        cart = _repository.Update(cart);

        var cartUpdated = await _unitOfWork.Complete();

        if (cartUpdated)
            throw new Exception("Failed to sync cart");

        return _mapper.Map<CartDto>(cart);
    }

}
