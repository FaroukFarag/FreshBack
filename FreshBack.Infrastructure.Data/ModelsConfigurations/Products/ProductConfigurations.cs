using FreshBack.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Products;

public class ProductConfigurations : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.NameEn)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.WeightInKg)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.MerchantId)
            .IsRequired();

        builder.Property(p => p.MerchantId)
            .IsRequired();

        builder.HasMany(p => p.ProductsBranches)
            .WithOne(bp => bp.Product)
            .HasForeignKey(bp => bp.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.CartItems)
            .WithOne(ci => ci.Product)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.ProductsOrders)
            .WithOne(po => po.Product)
            .HasForeignKey(po => po.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
