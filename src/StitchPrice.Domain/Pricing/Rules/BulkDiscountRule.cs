using StitchPrice.Domain.Enums;

namespace StitchPrice.Domain.Pricing.Rules;

public sealed class BulkDiscountRule : IPricingRule
{
    public bool IsMatch(PricingContext context) =>
        context.Settings.BulkDiscountEnabled && GetDiscountRate(context.Quantity) > 0;

    public PricingAdjustment Apply(PricingContext context, decimal runningSubtotal)
    {
        var rate = GetDiscountRate(context.Quantity);
        var discount = decimal.Round(runningSubtotal * rate, 2);

        return new PricingAdjustment
        {
            Name = "Bulk discount",
            Description = $"{rate * 100:F0}% discount for quantity {context.Quantity}",
            Amount = -discount,
            Type = PricingAdjustmentType.Discount,
            SortOrder = 6
        };
    }

    public static decimal GetDiscountRate(int quantity) => quantity switch
    {
        >= 50 => 0.20m,
        >= 25 => 0.15m,
        >= 10 => 0.10m,
        >= 5  => 0.05m,
        _     => 0m
    };
}
