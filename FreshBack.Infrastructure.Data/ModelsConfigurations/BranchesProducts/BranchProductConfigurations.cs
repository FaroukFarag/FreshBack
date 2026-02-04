using FreshBack.Domain.Models.BranchesProducts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.BranchesProducts;

public class BranchProductConfigurations : IEntityTypeConfiguration<BranchProduct>
{
    public void Configure(EntityTypeBuilder<BranchProduct> builder)
    {
        builder.HasKey(bp => new { bp.BranchId, bp.ProductId });

        builder.Property(bp => bp.Discount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(bp => bp.CreationDate)
            .IsRequired();

        builder.Property(bp => bp.ExpiryDate)
            .IsRequired();

        builder.Property(bp => bp.Quantity)
            .IsRequired();

        builder.Property(bp => bp.Views)
            .IsRequired();

        builder.Property(bp => bp.StartDeliveryDate)
            .IsRequired();

        builder.Property(bp => bp.EndDeliveryDate)
            .IsRequired();

        builder.Property(bp => bp.Status)
            .IsRequired();

        builder.HasOne(bp => bp.Branch)
            .WithMany(b => b.BranchesProducts)
            .HasForeignKey(bp => bp.BranchId);

        builder.HasOne(bp => bp.Product)
            .WithMany(p => p.ProductsBranches)
            .HasForeignKey(bp => bp.ProductId);
    }
}
