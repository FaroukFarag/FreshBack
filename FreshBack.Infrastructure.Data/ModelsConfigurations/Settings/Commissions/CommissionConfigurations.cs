using FreshBack.Domain.Models.Settings.Commissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Settings.Commissions;

public class CommissionConfigurations : IEntityTypeConfiguration<Commission>
{
    public void Configure(EntityTypeBuilder<Commission> builder)
    {
        builder.Property(c => c.Type);

        builder.Property(c => c.FixedAmount)
            .HasPrecision(18, 2);

        builder.HasMany(c => c.CategoryCommissions)
            .WithOne(cc => cc.Commission)
            .HasForeignKey(c => c.CommissionId);
    }
}
