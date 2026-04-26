using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;
using StitchPrice.Domain.Pricing;

namespace StitchPrice.UnitTests.Pricing;

internal static class PricingTestFactory
{
    public static PricingSettings DefaultSettings() => new()
    {
        PricePerThousandStitches = 10m,
        SetupFee = 20m,
        DigitizingFee = 30m,
        UrgencyMultiplier = 1.25m,
        DefaultMarkupPercentage = 40m,
        MinimumOrderPrice = 50m,
        ColorComplexityFeePerColor = 5m,
        BulkDiscountEnabled = true,
        CreatedAtUtc = DateTime.UtcNow,
        UpdatedAtUtc = DateTime.UtcNow
    };

    public static PricingContext DefaultContext(
        int quantity = 1,
        int stitchCount = 5000,
        int colorCount = 1,
        decimal garmentCost = 30m,
        bool requiresDigitizing = false,
        bool isUrgent = false,
        PricingSettings? settings = null) =>
        new()
        {
            ProductType = ProductType.TShirt,
            PlacementType = PlacementType.LeftChest,
            FabricType = FabricType.Cotton,
            Quantity = quantity,
            StitchCount = stitchCount,
            ColorCount = colorCount,
            GarmentCostPerItem = garmentCost,
            RequiresDigitizing = requiresDigitizing,
            IsUrgent = isUrgent,
            Settings = settings ?? DefaultSettings()
        };

    /// <summary>
    /// The canonical worked example from the spec:
    /// Hoodie × 10, 18k stitches, 4 colors, digitizing, not urgent, 40% markup.
    /// Expected: garment=350, stitches=180, colors=20, digitizing=30, setup=20 → subtotal=600
    /// Bulk 10%=60, net=540, markup 40%=216 → final=756 (not 745 from spec — see test comment).
    /// </summary>
    public static PricingContext WorkedExampleContext() =>
        new()
        {
            ProductType = ProductType.Hoodie,
            PlacementType = PlacementType.LeftChest,
            FabricType = FabricType.CottonPolyBlend,
            Quantity = 10,
            StitchCount = 18000,
            ColorCount = 4,
            GarmentCostPerItem = 35m,
            RequiresDigitizing = true,
            IsUrgent = false,
            Settings = DefaultSettings()
        };
}
