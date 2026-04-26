using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing.Rules;

public sealed class StitchCountRule : IPricingRule
{
    public bool IsMatch(PricingContext context) => true;

    public PricingAdjustment Apply(PricingContext context, decimal runningSubtotal)
    {
        var amount = (context.StitchCount / 1000m)
            * context.Settings.PricePerThousandStitches
            * context.Quantity;

        return new PricingAdjustment
        {
            Name = "Stitch cost",
            Description = $"{context.StitchCount:N0} stitches × {context.Settings.PricePerThousandStitches:F2} GEL per 1,000 × {context.Quantity} items",
            Amount = amount,
            Type = PricingAdjustmentType.Cost,
            SortOrder = 2
        };
    }
}
