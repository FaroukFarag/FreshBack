using FreshBack.Domain.Lookups.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.Lookups.Abstraction;

public class BaseLookupConfiguration<TEntity, TEnum>(string tableName) : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseLookup, new()
    where TEnum : Enum
{
    private readonly string _tableName = tableName;

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(_tableName);

        builder.HasKey(bl => bl.Id);

        builder.Property(bl => bl.Id)
           .ValueGeneratedNever();

        builder.Property(bl => bl.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasData(
            Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(bl => new TEntity { Id = Convert.ToInt32(bl), Name = bl.ToString() })
        );
    }
}
