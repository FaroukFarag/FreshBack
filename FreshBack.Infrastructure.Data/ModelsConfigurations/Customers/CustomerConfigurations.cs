using FreshBack.Domain.Models.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Customers;

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(c => c.Name)
            .IsRequired(false)
            .HasMaxLength(250);

        builder.Property(c => c.Email)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.HasMany(customer => customer.Carts)
            .WithOne(cart => cart.Customer)
            .HasForeignKey(cart => cart.CustomerId);

        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);
    }
}
