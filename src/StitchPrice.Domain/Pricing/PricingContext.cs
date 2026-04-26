using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing;

public sealed class PricingContext
{
    public required ProductType ProductType { get; init; }
    public required PlacementType PlacementType { get; init; }
    public required FabricType FabricType { get; init; }
    public required int Quantity { get; init; }
    public required int StitchCount { get; init; }
    public required int ColorCount { get; init; }
    public required decimal GarmentCostPerItem { get; init; }
    public required bool RequiresDigitizing { get; init; }
    public required bool IsUrgent { get; init; }
    public required PricingSettings Settings { get; init; }
    public ProductPricingProfile? ProductProfile { get; init; }
}
