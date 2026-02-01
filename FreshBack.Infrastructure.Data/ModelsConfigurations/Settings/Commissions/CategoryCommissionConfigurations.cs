using FreshBack.Domain.Models.Settings.Commissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.Commissions;

public class CategoryCommissionConfigurations :
    IEntityTypeConfiguration<CategoryCommission>
{
    public void Configure(EntityTypeBuilder<CategoryCommission> builder)
    {
        builder.Property(cc => cc.CommissionId)
            .IsRequired();

        builder.Property(cc => cc.CategoryId)
            .IsRequired();

        builder.Property(cc => cc.PercentageOfTotal)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(cc => cc.Commission)
            .WithMany(c => c.CategoryCommissions)
            .HasForeignKey(c => c.CommissionId);

        builder.HasOne(cc => cc.Category)
            .WithOne(c => c.CategoryCommission);
    }
}
