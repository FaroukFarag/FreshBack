using FreshBack.Domain.Models.Settings.Areas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.Areas;

public class AreaConfigurations : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(a => a.DeliveryFees)
            .IsRequired()
            .HasPrecision(18, 2);
    }
}
