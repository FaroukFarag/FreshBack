using FreshBack.Domain.Models.Addresses;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.BranchesFavorites;
using FreshBack.Domain.Models.Carts;
using FreshBack.Domain.Models.Categories;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Notifications;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.OrdersPhotos;
using FreshBack.Domain.Models.OtpCodes;
using FreshBack.Domain.Models.Products;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.Domain.Models.Roles;
using FreshBack.Domain.Models.Settings.Areas;
using FreshBack.Domain.Models.Settings.Users;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Addresses;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Branches;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Carts;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Categories;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Customers;
using FreshBack.Infrastructure.Data.ModelsConfigurations.CustomersBranchesFavorite;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Merchants;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Notifications;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Orders;
using FreshBack.Infrastructure.Data.ModelsConfigurations.OrdersPhotos;
using FreshBack.Infrastructure.Data.ModelsConfigurations.OtpCodes;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Products;
using FreshBack.Infrastructure.Data.ModelsConfigurations.ProductsOrders;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Roles;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.Areas;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreshBack.Infrastructure.Data.Context;

public class FreshBackDbContext(DbContextOptions options) : IdentityDbContext<User, Role, int>(options)
{
    public DbSet<Area> Areas { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<OrderPhoto> OrderPhotos { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductOrder> ProductsOrders { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }
    public DbSet<CustomerBranchFavorite> CustomersBranchesFavorite { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new RoleConfigurations());
        modelBuilder.ApplyConfiguration(new AreaConfigurations());
        modelBuilder.ApplyConfiguration(new ReviewConfigurations());
        modelBuilder.ApplyConfiguration(new CategoryConfigurations());
        modelBuilder.ApplyConfiguration(new MerchantConfigurations());
        modelBuilder.ApplyConfiguration(new BranchConfigurations());
        modelBuilder.ApplyConfiguration(new ProductConfigurations());
        modelBuilder.ApplyConfiguration(new CartConfigurations());
        modelBuilder.ApplyConfiguration(new CartItemConfigurations());
        modelBuilder.ApplyConfiguration(new OrderPhotoConfigurations());
        modelBuilder.ApplyConfiguration(new OrderConfigurations());
        modelBuilder.ApplyConfiguration(new ProductOrderConfigurations());
        modelBuilder.ApplyConfiguration(new NotificationConfigurations());
        modelBuilder.ApplyConfiguration(new AddressConfigurations());
        modelBuilder.ApplyConfiguration(new CustomerConfigurations());
        modelBuilder.ApplyConfiguration(new OtpCodeConfigurations());
        modelBuilder.ApplyConfiguration(new CustomerBranchFavoriteConfigurations());
    }
}
