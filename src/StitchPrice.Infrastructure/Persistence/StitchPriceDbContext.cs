using Microsoft.EntityFrameworkCore;
using StitchPrice.Domain.Entities;

namespace StitchPrice.Infrastructure.Persistence;

public sealed class StitchPriceDbContext(DbContextOptions<StitchPriceDbContext> options)
    : DbContext(options)
{
    public DbSet<PricingQuote> Quotes => Set<PricingQuote>();
    public DbSet<PricingBreakdownItem> BreakdownItems => Set<PricingBreakdownItem>();
    public DbSet<PricingSettings> Settings => Set<PricingSettings>();
    public DbSet<ProductPricingProfile> ProductProfiles => Set<ProductPricingProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StitchPriceDbContext).Assembly);
    }
}
