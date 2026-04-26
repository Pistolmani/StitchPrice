namespace StitchPrice.Domain.Pricing;

public sealed class PricingResult
{
    public required decimal Subtotal { get; init; }
    public required decimal DiscountAmount { get; init; }
    public required decimal MarkupAmount { get; init; }
    public required decimal FinalPrice { get; init; }
    public required decimal PricePerItem { get; init; }
    public required decimal ProfitMarginPercentage { get; init; }
    public required IReadOnlyList<PricingAdjustment> Adjustments { get; init; }
}
