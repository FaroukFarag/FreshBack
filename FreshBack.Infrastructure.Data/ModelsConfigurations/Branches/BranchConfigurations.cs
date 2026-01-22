using FreshBack.Domain.Models.Branches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Branches;

public class BranchConfigurations : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(b => b.NameEn)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(b => b.Neighborhood)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.NeighborhoodEn)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Location)
        .HasColumnType("geography");

        builder.Property(b => b.OpeningTime)
            .IsRequired();

        builder.Property(b => b.ClosingTime)
            .IsRequired();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Branches_ClosingTime_After_OpeningTime",
                "[ClosingTime] > [OpeningTime]"
            );
        });

        builder.Property(b => b.Status)
            .IsRequired();

        builder.Property(b => b.AreaId)
            .IsRequired();

        builder.Property(b => b.MerchantId)
            .IsRequired();

        builder.Property(b => b.CategoryId)
            .IsRequired();

        builder.HasOne(b => b.Area)
            .WithMany(c => c.Branches)
            .HasForeignKey(b => b.AreaId);

        builder.HasOne(b => b.Merchant)
            .WithMany(m => m.Branches)
            .HasForeignKey(b => b.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Category)
            .WithMany(c => c.Branches)
            .HasForeignKey(b => b.CategoryId);

        builder.HasMany(b => b.Products)
            .WithOne(p => p.Branch)
            .HasForeignKey(p => p.BranchId);

        builder.HasMany(b => b.CustomersBranchesFavorite)
            .WithOne(cbf => cbf.Branch)
            .HasForeignKey(b => b.BranchId);

        builder.HasMany(m => m.Reviews)
            .WithOne(r => r.Branch)
            .HasForeignKey(r => r.BranchId);
    }
}
