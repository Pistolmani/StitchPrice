using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing.Rules;

public sealed class DigitizingFeeRule : IPricingRule
{
    public bool IsMatch(PricingContext context) => context.RequiresDigitizing;

    public PricingAdjustment Apply(PricingContext context, decimal runningSubtotal) =>
        new()
        {
            Name = "Digitizing fee",
            Description = "One-time design digitizing setup",
            Amount = context.Settings.DigitizingFee,
            Type = PricingAdjustmentType.Fee,
            SortOrder = 4
        };
}
