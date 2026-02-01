using FreshBack.Domain.Models.Branches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Branches;

public class ReviewConfigurations : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(r => r.Rating)
            .IsRequired();

        builder.Property(r => r.Date)
            .IsRequired();

        builder.Property(r => r.CustomerId)
            .IsRequired();

        builder.Property(r => r.MerchantId)
            .IsRequired();

        builder.Property(r => r.BranchId)
            .IsRequired();

        builder.Property(r => r.OrderId)
            .IsRequired();

        builder.ToTable(t =>
        t.HasCheckConstraint(
            "CK_Review_Rating",
            "[Rating] BETWEEN 1 AND 5")); ;

        builder.HasOne(r => r.Branch)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.BranchId);

        builder.HasOne(r => r.Merchant)
            .WithMany(m => m.Reviews)
            .HasForeignKey(r => r.MerchantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Order)
            .WithOne(m => m.Review)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
