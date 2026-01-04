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

        builder.Property(p => p.Discount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.CreationDate)
            .IsRequired();

        builder.Property(p => p.ExpiryDate)
            .IsRequired();

        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(p => p.Views)
            .IsRequired();

        builder.Property(p => p.StartDeliveryDate)
            .IsRequired();

        builder.Property(p => p.EndDeliveryDate)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.MerchantId)
            .IsRequired();

        builder.HasMany(p => p.ProductsOrders)
            .WithOne(po => po.Product)
            .HasForeignKey(po => po.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
