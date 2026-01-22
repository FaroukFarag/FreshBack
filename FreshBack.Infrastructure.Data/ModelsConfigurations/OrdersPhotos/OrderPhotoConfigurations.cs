using FreshBack.Domain.Models.OrdersPhotos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.OrdersPhotos;

public class OrderPhotoConfigurations : IEntityTypeConfiguration<OrderPhoto>
{
    public void Configure(EntityTypeBuilder<OrderPhoto> builder)
    {
        builder.Property(op => op.OrderId)
            .IsRequired();

        builder.Property(op => op.BranchId)
        .IsRequired();

        builder.HasOne(op => op.Branch)
            .WithMany(b => b.OrderPhotos)
            .HasForeignKey(op => op.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
