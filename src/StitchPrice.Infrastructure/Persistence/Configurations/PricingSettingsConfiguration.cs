using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StitchPrice.Domain.Entities;

namespace StitchPrice.Infrastructure.Persistence.Configurations;

internal sealed class PricingSettingsConfiguration : IEntityTypeConfiguration<PricingSettings>
{
    public void Configure(EntityTypeBuilder<PricingSettings> builder)
    {
        builder.ToTable("pricing_settings");

        builder.HasKey(s => s.Id);
        // Singleton row: Id is always 1. Assigned in application, not auto-generated.
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.Property(s => s.PricePerThousandStitches).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(s => s.SetupFee).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(s => s.DigitizingFee).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(s => s.UrgencyMultiplier).HasColumnType("numeric(7,4)").IsRequired();
        builder.Property(s => s.DefaultMarkupPercentage).HasColumnType("numeric(7,2)").IsRequired();
        builder.Property(s => s.MinimumOrderPrice).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(s => s.ColorComplexityFeePerColor).HasColumnType("numeric(18,2)").IsRequired();

        builder.Property(s => s.CreatedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
        builder.Property(s => s.UpdatedAtUtc).HasColumnType("timestamp with time zone").IsRequired();
    }
}
