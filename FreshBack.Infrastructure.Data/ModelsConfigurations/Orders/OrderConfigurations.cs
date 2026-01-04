using FreshBack.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Orders;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.Number)
            .IsRequired();

        builder.Property(o => o.CreationDate)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired();

        builder.Property(o => o.PaymentMethod)
            .IsRequired();

        builder.Property(o => o.Price)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(o => o.Discount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(o => o.Fees)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(o => o.MerchantId)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(o => o.Merchant)
            .WithMany(m => m.Orders)
            .HasForeignKey(o => o.MerchantId);

        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);

        builder.HasMany(o => o.ProductsOrders)
            .WithOne(po => po.Order)
            .HasForeignKey(po => po.OrderId);
    }
}
