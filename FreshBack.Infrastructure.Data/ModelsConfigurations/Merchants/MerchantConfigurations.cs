using FreshBack.Domain.Models.Merchants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Merchants;

public class MerchantConfigurations : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(m => m.Description)
            .IsRequired();

        builder.Property(m => m.Story)
            .IsRequired();

        builder.Property(m => m.PhoneNumber)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(m => m.AreaId)
            .IsRequired();

        builder.HasOne(m => m.Category)
            .WithMany(c => c.Merchants)
            .HasForeignKey(m => m.CategoryId);

        builder.HasMany(m => m.Reviews)
            .WithOne(r => r.Merchant)
            .HasForeignKey(r => r.MerchantId);

        builder.HasMany(m => m.Branches)
            .WithOne(b => b.Merchant)
            .HasForeignKey(b => b.MerchantId);

        builder.HasMany(m => m.Orders)
            .WithOne(o => o.Merchant)
            .HasForeignKey(o => o.MerchantId);
    }
}
