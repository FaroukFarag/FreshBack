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

        builder.Property(m => m.NameEn)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(m => m.Description)
            .IsRequired();

        builder.Property(m => m.DescriptionEn)
            .IsRequired();

        builder.Property(m => m.Story)
            .IsRequired();

        builder.Property(m => m.StoryEn)
            .IsRequired();

        builder.Property(m => m.PhoneNumber)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(m => m.AreaId)
            .IsRequired();

        builder.HasMany(m => m.Branches)
            .WithOne(b => b.Merchant)
            .HasForeignKey(b => b.MerchantId);

        builder.HasMany(m => m.Orders)
            .WithOne(o => o.Merchant)
            .HasForeignKey(o => o.MerchantId);
    }
}
