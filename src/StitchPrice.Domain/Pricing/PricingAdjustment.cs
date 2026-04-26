using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing;

public sealed class PricingAdjustment
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required decimal Amount { get; init; }
    public required PricingAdjustmentType Type { get; init; }
    public required int SortOrder { get; init; }
}
