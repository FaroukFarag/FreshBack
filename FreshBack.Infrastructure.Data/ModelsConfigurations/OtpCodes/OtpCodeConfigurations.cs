using FreshBack.Domain.Models.OtpCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreshBack.Infrastructure.Data.ModelsConfigurations.OtpCodes;

public class OtpCodeConfigurations : IEntityTypeConfiguration<OtpCode>
{
    public void Configure(EntityTypeBuilder<OtpCode> builder)
    {
        builder.Property(otp => otp.PhoneNumber)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(otp => otp.CodeHash)
            .IsRequired();

        builder.Property(otp => otp.ExpireAt)
            .IsRequired();

        builder.Property(otp => otp.IsUsed)
            .IsRequired();
    }
}
