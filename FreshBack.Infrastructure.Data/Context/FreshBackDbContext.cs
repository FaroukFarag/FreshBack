using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.Products;
using FreshBack.Domain.Models.Roles;
using FreshBack.Domain.Models.Settings.Areas;
using FreshBack.Domain.Models.Settings.Users;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Merchants;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Orders;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Products;
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
    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new RoleConfigurations());
        modelBuilder.ApplyConfiguration(new AreaConfigurations());
        modelBuilder.ApplyConfiguration(new ReviewConfigurations());
        modelBuilder.ApplyConfiguration(new MerchantConfigurations());
        modelBuilder.ApplyConfiguration(new ProductConfigurations());
        modelBuilder.ApplyConfiguration(new OrderConfigurations());
    }
}
