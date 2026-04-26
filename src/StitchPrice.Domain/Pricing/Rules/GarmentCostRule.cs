using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing.Rules;

public sealed class GarmentCostRule : IPricingRule
{
    public bool IsMatch(PricingContext context) => true;

    public PricingAdjustment Apply(PricingContext context, decimal runningSubtotal)
    {
        var amount = context.GarmentCostPerItem * context.Quantity;
        return new PricingAdjustment
        {
            Name = "Garment cost",
            Description = $"{context.GarmentCostPerItem:F2} GEL × {context.Quantity} items",
            Amount = amount,
            Type = PricingAdjustmentType.Cost,
            SortOrder = 1
        };
    }
}
