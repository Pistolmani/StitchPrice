using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing.Rules;

public sealed class ColorComplexityRule : IPricingRule
{
    public bool IsMatch(PricingContext context) => context.ColorCount > 0;

    public PricingAdjustment Apply(PricingContext context, decimal runningSubtotal)
    {
        var amount = context.ColorCount * context.Settings.ColorComplexityFeePerColor;
        return new PricingAdjustment
        {
            Name = "Color complexity fee",
            Description = $"{context.ColorCount} thread color{(context.ColorCount == 1 ? "" : "s")} × {context.Settings.ColorComplexityFeePerColor:F2} GEL",
            Amount = amount,
            Type = PricingAdjustmentType.Fee,
            SortOrder = 3
        };
    }
}
