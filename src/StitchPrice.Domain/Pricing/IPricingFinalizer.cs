namespace StitchPrice.Domain.Pricing;

/// <summary>
/// Post-calculation constraint applied after all rules have run.
/// Unlike IPricingRule, a finalizer receives the fully accumulated total
/// and may adjust it to satisfy a business constraint (e.g. minimum order price).
/// </summary>
public interface IPricingFinalizer
{
    PricingAdjustment? Apply(decimal totalAfterRules, PricingContext context);
}
