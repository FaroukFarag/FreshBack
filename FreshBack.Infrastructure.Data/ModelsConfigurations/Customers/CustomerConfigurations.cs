using FreshBack.Domain.Models.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Customers;

public class CustomerConfigurations : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(c => c.Country)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.City)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Area)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Street)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.BuildingNo)
           .IsRequired();

        builder.Property(c => c.Longitude)
           .IsRequired()
           .HasPrecision(18, 18);

        builder.Property(c => c.Latitude)
           .IsRequired()
           .HasPrecision(18, 18);

        builder.HasMany(customer => customer.Carts)
            .WithOne(cart => cart.Customer)
            .HasForeignKey(cart => cart.CustomerId);

        builder.HasMany(c => c.Orders)
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);
    }
}
