using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing.Rules;

public sealed class MarkupRule : IPricingRule
{
    public bool IsMatch(PricingContext context) => GetMarkupPercentage(context) > 0;

    public PricingAdjustment Apply(PricingContext context, decimal runningSubtotal)
    {
        var markupPct = GetMarkupPercentage(context);
        var markup = decimal.Round(runningSubtotal * (markupPct / 100m), 2);

        return new PricingAdjustment
        {
            Name = "Markup",
            Description = $"{markupPct:F0}% business profit markup",
            Amount = markup,
            Type = PricingAdjustmentType.Markup,
            SortOrder = 8
        };
    }

    private static decimal GetMarkupPercentage(PricingContext ctx) =>
        ctx.ProductProfile?.DefaultMarkupPercentage ?? ctx.Settings.DefaultMarkupPercentage;
}
