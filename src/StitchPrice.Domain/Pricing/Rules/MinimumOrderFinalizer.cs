using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing.Rules;

public sealed class MinimumOrderFinalizer : IPricingFinalizer
{
    public PricingAdjustment? Apply(decimal totalAfterRules, PricingContext context)
    {
        var minimum = context.Settings.MinimumOrderPrice;
        if (totalAfterRules >= minimum)
            return null;

        return new PricingAdjustment
        {
            Name = "Minimum order adjustment",
            Description = $"Order raised to minimum of {minimum:F2} GEL",
            Amount = minimum - totalAfterRules,
            Type = PricingAdjustmentType.Fee,
            SortOrder = 9
        };
    }
}
