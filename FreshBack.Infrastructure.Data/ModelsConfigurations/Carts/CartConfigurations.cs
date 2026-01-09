using FreshBack.Domain.Models.Carts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Carts;

public class CartConfigurations : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.Property(c => c.CustomerId)
            .IsRequired();

        builder.HasOne(cart => cart.Customer)
            .WithMany(customer => customer.Carts)
            .HasForeignKey(cart => cart.CustomerId);
    }
}
