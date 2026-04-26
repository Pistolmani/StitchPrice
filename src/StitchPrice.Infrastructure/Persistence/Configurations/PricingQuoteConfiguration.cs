using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;

namespace StitchPrice.Infrastructure.Persistence.Configurations;

internal sealed class PricingQuoteConfiguration : IEntityTypeConfiguration<PricingQuote>
{
    public void Configure(EntityTypeBuilder<PricingQuote> builder)
    {
        builder.ToTable("pricing_quotes");

        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).ValueGeneratedNever(); // Guid assigned in application

        builder.Property(q => q.ProductType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(q => q.PlacementType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(q => q.FabricType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(q => q.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(q => q.GarmentCostPerItem).HasColumnType("numeric(18,2)");
        builder.Property(q => q.SetupFee).HasColumnType("numeric(18,2)");
        builder.Property(q => q.DigitizingFee).HasColumnType("numeric(18,2)");
        builder.Property(q => q.Subtotal).HasColumnType("numeric(18,2)");
        builder.Property(q => q.DiscountAmount).HasColumnType("numeric(18,2)");
        builder.Property(q => q.MarkupAmount).HasColumnType("numeric(18,2)");
        builder.Property(q => q.FinalPrice).HasColumnType("numeric(18,2)");
        builder.Property(q => q.PricePerItem).HasColumnType("numeric(18,2)");
        builder.Property(q => q.ProfitMarginPercentage).HasColumnType("numeric(7,1)");

        builder.Property(q => q.Note).HasMaxLength(500);

        builder.Property(q => q.CreatedAtUtc)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasMany(q => q.BreakdownItems)
            .WithOne(b => b.Quote)
            .HasForeignKey(b => b.PricingQuoteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
