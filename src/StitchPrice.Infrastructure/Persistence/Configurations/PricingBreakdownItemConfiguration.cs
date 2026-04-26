using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StitchPrice.Domain.Entities;

namespace StitchPrice.Infrastructure.Persistence.Configurations;

internal sealed class PricingBreakdownItemConfiguration : IEntityTypeConfiguration<PricingBreakdownItem>
{
    public void Configure(EntityTypeBuilder<PricingBreakdownItem> builder)
    {
        builder.ToTable("pricing_breakdown_items");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).UseIdentityAlwaysColumn();

        builder.Property(b => b.Name).HasMaxLength(100).IsRequired();
        builder.Property(b => b.Description).HasMaxLength(300).IsRequired();

        builder.Property(b => b.Amount).HasColumnType("numeric(18,2)").IsRequired();

        builder.Property(b => b.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(b => b.PricingQuoteId);
    }
}
