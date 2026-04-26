using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing.Rules;

public sealed class UrgencyFeeRule : IPricingRule
{
    public bool IsMatch(PricingContext context) => context.IsUrgent;

    public PricingAdjustment Apply(PricingContext context, decimal runningSubtotal)
    {
        var surchargeRate = context.Settings.UrgencyMultiplier - 1m;
        var fee = decimal.Round(runningSubtotal * surchargeRate, 2);

        return new PricingAdjustment
        {
            Name = "Urgency fee",
            Description = $"+{surchargeRate * 100:F0}% rush order surcharge",
            Amount = fee,
            Type = PricingAdjustmentType.Fee,
            SortOrder = 7
        };
    }
}
