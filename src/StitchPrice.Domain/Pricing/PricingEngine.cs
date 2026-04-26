using StitchPrice.Domain.Pricing.Rules;

namespace StitchPrice.Domain.Pricing;

public sealed class PricingEngine
{
    private readonly IReadOnlyList<IPricingRule> _rules;
    private readonly IReadOnlyList<IPricingFinalizer> _finalizers;

    public PricingEngine(IEnumerable<IPricingRule> rules, IEnumerable<IPricingFinalizer> finalizers)
    {
        _rules = [.. rules];
        _finalizers = [.. finalizers];
    }

    /// <summary>
    /// Creates an engine with the standard rule set. Use this when DI is not available.
    /// When running under ASP.NET Core, register rules via DI and inject instead.
    /// </summary>
    public static PricingEngine CreateDefault() => new(
        [
            new GarmentCostRule(),
            new StitchCountRule(),
            new ColorComplexityRule(),
            new DigitizingFeeRule(),
            new SetupFeeRule(),
            new BulkDiscountRule(),  // discount on base cost, before urgency surcharge
            new UrgencyFeeRule(),
            new MarkupRule()
        ],
        [new MinimumOrderFinalizer()]);

    public PricingResult Calculate(PricingContext context)
    {
        var adjustments = new List<PricingAdjustment>();
        var runningSubtotal = 0m;

        foreach (var rule in _rules)
        {
            if (!rule.IsMatch(context))
                continue;

            var adjustment = rule.Apply(context, runningSubtotal);
            adjustments.Add(adjustment);
            runningSubtotal += adjustment.Amount;
        }

        foreach (var finalizer in _finalizers)
        {
            var adjustment = finalizer.Apply(runningSubtotal, context);
            if (adjustment is not null)
            {
                adjustments.Add(adjustment);
                runningSubtotal += adjustment.Amount;
            }
        }

        var finalPrice = decimal.Round(runningSubtotal, 2);
        var pricePerItem = context.Quantity > 0
            ? decimal.Round(finalPrice / context.Quantity, 2)
            : 0m;

        var costs = adjustments
            .Where(a => a.Type is Enums.PricingAdjustmentType.Cost)
            .Sum(a => a.Amount);
        var discounts = Math.Abs(adjustments
            .Where(a => a.Type is Enums.PricingAdjustmentType.Discount)
            .Sum(a => a.Amount));
        var markup = adjustments
            .Where(a => a.Type is Enums.PricingAdjustmentType.Markup)
            .Sum(a => a.Amount);

        var netCost = costs - discounts;
        var profitMargin = netCost > 0
            ? decimal.Round(markup / (netCost + markup) * 100m, 1)
            : 0m;

        // Subtotal = all costs + fees (not discounts, not markup)
        var subtotal = adjustments
            .Where(a => a.Type is Enums.PricingAdjustmentType.Cost or Enums.PricingAdjustmentType.Fee)
            .Sum(a => a.Amount);

        return new PricingResult
        {
            Subtotal = subtotal,
            DiscountAmount = discounts,
            MarkupAmount = markup,
            FinalPrice = finalPrice,
            PricePerItem = pricePerItem,
            ProfitMarginPercentage = profitMargin,
            Adjustments = adjustments.OrderBy(a => a.SortOrder).ToList()
        };
    }
}
