using FreshBack.Domain.Interfaces.Repositories.Abstraction;
using FreshBack.Domain.Models.Products;

namespace FreshBack.Domain.Interfaces.Repositories.Products;

public interface IProductRepository : IBaseRepository<Product, int>
{
}
