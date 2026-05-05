using FreshBack.Common.Interfaces.Merchants;
using FreshBack.Common.Interfaces.Settings.Users;
using FreshBack.Domain.Models.Abstraction;
using FreshBack.Domain.Models.Addresses;
using FreshBack.Domain.Models.Branches;
using FreshBack.Domain.Models.BranchesFavorites;
using FreshBack.Domain.Models.BranchesProducts;
using FreshBack.Domain.Models.Carts;
using FreshBack.Domain.Models.Categories;
using FreshBack.Domain.Models.Customers;
using FreshBack.Domain.Models.DevicesTokens;
using FreshBack.Domain.Models.Merchants;
using FreshBack.Domain.Models.Notifications;
using FreshBack.Domain.Models.Orders;
using FreshBack.Domain.Models.OtpCodes;
using FreshBack.Domain.Models.Products;
using FreshBack.Domain.Models.ProductsOrders;
using FreshBack.Domain.Models.Roles;
using FreshBack.Domain.Models.Settings.Areas;
using FreshBack.Domain.Models.Settings.Commissions;
using FreshBack.Domain.Models.Settings.PaymentMethods;
using FreshBack.Domain.Models.Settings.Users;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Addresses;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Branches;
using FreshBack.Infrastructure.Data.ModelsConfigurations.BranchesProducts;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Carts;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Categories;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Customers;
using FreshBack.Infrastructure.Data.ModelsConfigurations.CustomersBranchesFavorite;
using FreshBack.Infrastructure.Data.ModelsConfigurations.DevicesTokens;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Merchants;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Notifications;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Orders;
using FreshBack.Infrastructure.Data.ModelsConfigurations.OtpCodes;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Products;
using FreshBack.Infrastructure.Data.ModelsConfigurations.ProductsOrders;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Roles;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.Areas;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.Commissions;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.PaymentMethods;
using FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace FreshBack.Infrastructure.Data.Context;

public class FreshBackDbContext(
    DbContextOptions options,
    IUserContextService userContextService) : IdentityDbContext<User, Role, int>(options)
{
    private readonly IUserContextService _userContextService = userContextService;

    public DbSet<Area> Areas { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<BranchProduct> BranchesProducts { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductOrder> ProductsOrders { get; set; }
    public DbSet<ReviewImage> Feedbacks { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<DeviceToken> DevicesTokens { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }
    public DbSet<CustomerBranchFavorite> CustomersBranchesFavorite { get; set; }
    public DbSet<Commission> Commissions { get; set; }
    public DbSet<CategoryCommission> CategoryCommissions { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }

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
        modelBuilder.ApplyConfiguration(new OrderConfigurations());
        modelBuilder.ApplyConfiguration(new ProductOrderConfigurations());
        modelBuilder.ApplyConfiguration(new BranchProductConfigurations());
        modelBuilder.ApplyConfiguration(new NotificationConfigurations());
        modelBuilder.ApplyConfiguration(new AddressConfigurations());
        modelBuilder.ApplyConfiguration(new CustomerConfigurations());
        modelBuilder.ApplyConfiguration(new DeviceTokenConfigurations());
        modelBuilder.ApplyConfiguration(new OtpCodeConfigurations());
        modelBuilder.ApplyConfiguration(new CustomerBranchFavoriteConfigurations());
        modelBuilder.ApplyConfiguration(new CommissionConfigurations());
        modelBuilder.ApplyConfiguration(new CategoryCommissionConfigurations());
        modelBuilder.ApplyConfiguration(new PaymentMethodConfigurations());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            if (typeof(IMerchantEntity).IsAssignableFrom(clrType))
            {
                var method = typeof(FreshBackDbContext)
                    .GetMethod(nameof(SetMerchantFilter), BindingFlags.NonPublic |
                        BindingFlags.Instance)
                    ?.MakeGenericMethod(clrType);

                method?.Invoke(this, [modelBuilder]);
            }
        }
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        SetAuditFields();

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        SetAuditFields();

        return base.SaveChanges();
    }

    private void SetAuditFields()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State ==
                EntityState.Modified);

        var userId = _userContextService.GetUserId();
        var now = DateTime.Now;

        foreach (var entry in entries)
        {
            if (!IsAuditableEntity(entry.Entity))
                continue;

            var primaryKeyType = GetPrimaryKeyType(entry.Entity);

            if (primaryKeyType == null)
                continue;

            var convertedUserId = Convert.ChangeType(userId, primaryKeyType);

            if (entry.State == EntityState.Added)
            {
                SetPropertyValue(entry, "CreatedBy", convertedUserId);
                SetPropertyValue(entry, "CreatedAt", now);
            }

            SetPropertyValue(entry, "LastModifiedBy", convertedUserId);
            SetPropertyValue(entry, "LastModifiedAt", now);
        }
    }

    private static bool IsAuditableEntity<TEntity>(TEntity entity)
    {
        var baseType = entity!.GetType().BaseType;

        while (baseType != null)
        {
            if (baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == typeof(BaseAuditModel<>))
            {
                return true;
            }

            baseType = baseType.BaseType;
        }

        return false;
    }

    private static Type? GetPrimaryKeyType<TEntity>(TEntity entity)
    {
        var baseType = entity!.GetType().BaseType;

        while (baseType != null)
        {
            if (baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == typeof(BaseAuditModel<>))
            {
                return baseType.GetGenericArguments()[0];
            }

            baseType = baseType.BaseType;
        }

        return null;
    }

    private static void SetPropertyValue(EntityEntry entry, string propertyName, object? value)
    {
        var property = entry.Property(propertyName);

        if (property != null)
        {
            property.CurrentValue = value;
        }
    }

    private void SetMerchantFilter<TEntity>(ModelBuilder builder) where TEntity : class,
        IMerchantEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(e =>
            _userContextService.IsAdmin() ||
            !_userContextService.IsAuthenticated() ||
            !_userContextService.HasMerchantId() ||
            e.MerchantId == _userContextService.GetMerchantId());
    }
}
