using FreshBack.Domain.Models.DevicesTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.DevicesTokens;

public class DeviceTokenConfigurations : IEntityTypeConfiguration<DeviceToken>
{
    public void Configure(EntityTypeBuilder<DeviceToken> builder)
    {
        builder.Property(dt => dt.Token)
            .IsRequired();

        builder.Property(dt => dt.IsActive)
            .IsRequired();

        builder.Property(dt => dt.CustomerId)
            .IsRequired();

        builder.HasOne(dt => dt.Customer)
            .WithMany(c => c.DevicesTokens)
            .HasForeignKey(dt => dt.CustomerId);
    }
}
