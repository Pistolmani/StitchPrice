using Microsoft.EntityFrameworkCore;
using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;

namespace StitchPrice.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(StitchPriceDbContext db)
    {
        await db.Database.MigrateAsync();

        await SeedSettingsAsync(db);
        await SeedProductProfilesAsync(db);
    }

    private static async Task SeedSettingsAsync(StitchPriceDbContext db)
    {
        if (await db.Settings.AnyAsync())
            return;

        var defaults = PricingSettings.Default();
        defaults.Id = 1;
        await db.Settings.AddAsync(defaults);
        await db.SaveChangesAsync();
    }

    private static async Task SeedProductProfilesAsync(StitchPriceDbContext db)
    {
        if (await db.ProductProfiles.AnyAsync())
            return;

        var profiles = new[]
        {
            new ProductPricingProfile { ProductType = ProductType.TShirt,  DefaultGarmentCost = 15m,  DefaultMarkupPercentage = 40m, DifficultyMultiplier = 1.0m, IsActive = true },
            new ProductPricingProfile { ProductType = ProductType.Hoodie,  DefaultGarmentCost = 35m,  DefaultMarkupPercentage = 40m, DifficultyMultiplier = 1.1m, IsActive = true },
            new ProductPricingProfile { ProductType = ProductType.Polo,    DefaultGarmentCost = 25m,  DefaultMarkupPercentage = 40m, DifficultyMultiplier = 1.0m, IsActive = true },
            new ProductPricingProfile { ProductType = ProductType.Cap,     DefaultGarmentCost = 12m,  DefaultMarkupPercentage = 45m, DifficultyMultiplier = 0.9m, IsActive = true },
            new ProductPricingProfile { ProductType = ProductType.Patch,   DefaultGarmentCost = 2m,   DefaultMarkupPercentage = 60m, DifficultyMultiplier = 0.8m, IsActive = true },
            new ProductPricingProfile { ProductType = ProductType.Sweater, DefaultGarmentCost = 30m,  DefaultMarkupPercentage = 40m, DifficultyMultiplier = 1.1m, IsActive = true },
            new ProductPricingProfile { ProductType = ProductType.Jacket,  DefaultGarmentCost = 60m,  DefaultMarkupPercentage = 35m, DifficultyMultiplier = 1.3m, IsActive = true },
            new ProductPricingProfile { ProductType = ProductType.Custom,  DefaultGarmentCost = 20m,  DefaultMarkupPercentage = 50m, DifficultyMultiplier = 1.2m, IsActive = true },
        };

        await db.ProductProfiles.AddRangeAsync(profiles);
        await db.SaveChangesAsync();
    }
}
