using FreshBack.Domain.Models.BranchesFavorites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.CustomersBranchesFavorite;

public class CustomerBranchFavoriteConfigurations :
    IEntityTypeConfiguration<CustomerBranchFavorite>
{
    public void Configure(EntityTypeBuilder<CustomerBranchFavorite> builder)
    {
        builder.HasKey(cbf => new { cbf.CustomerId, cbf.BranchId });


        builder.HasOne(cbf => cbf.Customer)
            .WithMany(c => c.CustomersBranchesFavorite)
            .HasForeignKey(cbf => cbf.CustomerId);

        builder.HasOne(cbf => cbf.Branch)
            .WithMany(b => b.CustomersBranchesFavorite)
            .HasForeignKey(cbf => cbf.BranchId);
    }
}
