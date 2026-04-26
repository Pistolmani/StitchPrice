namespace StitchPrice.Domain.Pricing;

public interface IPricingRule
{
    bool IsMatch(PricingContext context);

    /// <param name="runningSubtotal">Sum of all adjustments applied so far. Rules that depend
    /// on prior results (urgency, discount, markup) use this instead of re-deriving.</param>
    PricingAdjustment Apply(PricingContext context, decimal runningSubtotal);
}
