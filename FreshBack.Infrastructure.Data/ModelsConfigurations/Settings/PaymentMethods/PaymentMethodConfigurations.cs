using FreshBack.Domain.Models.Settings.PaymentMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.PaymentMethods;

public class PaymentMethodConfigurations : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.Property(pm => pm.NameAr)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pm => pm.NameEn)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pm => pm.IsActive)
            .IsRequired();
    }
}
