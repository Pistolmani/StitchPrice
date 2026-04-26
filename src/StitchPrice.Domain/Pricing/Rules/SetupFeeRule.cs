using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing.Rules;

public sealed class SetupFeeRule : IPricingRule
{
    public bool IsMatch(PricingContext context) => true;

    public PricingAdjustment Apply(PricingContext context, decimal runningSubtotal) =>
        new()
        {
            Name = "Setup fee",
            Description = "Machine and hoop setup",
            Amount = context.Settings.SetupFee,
            Type = PricingAdjustmentType.Fee,
            SortOrder = 5
        };
}
