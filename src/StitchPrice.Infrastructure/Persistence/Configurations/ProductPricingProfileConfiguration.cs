using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StitchPrice.Domain.Entities;

namespace StitchPrice.Infrastructure.Persistence.Configurations;

internal sealed class ProductPricingProfileConfiguration : IEntityTypeConfiguration<ProductPricingProfile>
{
    public void Configure(EntityTypeBuilder<ProductPricingProfile> builder)
    {
        builder.ToTable("product_pricing_profiles");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityAlwaysColumn();

        builder.Property(p => p.ProductType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(p => p.ProductType).IsUnique();

        builder.Property(p => p.DefaultGarmentCost).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(p => p.DefaultMarkupPercentage).HasColumnType("numeric(7,2)").IsRequired();
        builder.Property(p => p.DifficultyMultiplier).HasColumnType("numeric(7,4)").IsRequired();
    }
}
