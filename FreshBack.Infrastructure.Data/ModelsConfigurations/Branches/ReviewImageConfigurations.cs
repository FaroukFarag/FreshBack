using FreshBack.Domain.Models.Branches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Branches;

public class ReviewImageConfigurations : IEntityTypeConfiguration<ReviewImage>
{
    public void Configure(EntityTypeBuilder<ReviewImage> builder)
    {
        builder.Property(ri => ri.ReviewId)
            .IsRequired();

        builder.HasOne(ri => ri.Review)
            .WithMany(r => r.ReviewImages)
            .HasForeignKey(ri => ri.ReviewId);
    }
}
