using FreshBack.Domain.Models.ProductsOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.ProductsOrders;

public class ProductOrderConfigurations : IEntityTypeConfiguration<ProductOrder>
{
    public void Configure(EntityTypeBuilder<ProductOrder> builder)
    {
        builder.HasKey(agd => new { agd.ProductId, agd.OrderId });

        builder.Property(po => po.Quantity)
            .IsRequired();

        builder.HasOne(po => po.Product)
            .WithMany(p => p.ProductsOrders)
            .HasForeignKey(po => po.ProductId);

        builder.HasOne(po => po.Order)
            .WithMany(o => o.ProductsOrders)
            .HasForeignKey(po => po.OrderId);
    }
}
