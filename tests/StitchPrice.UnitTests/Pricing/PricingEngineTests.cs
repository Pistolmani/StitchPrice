using FluentAssertions;
using StitchPrice.Domain.Enums;
using StitchPrice.Domain.Pricing;

namespace StitchPrice.UnitTests.Pricing;

public sealed class PricingEngineTests
{
    private readonly PricingEngine _engine = PricingEngine.CreateDefault();

    [Fact]
    public void Calculate_WorkedExample_ReturnsCorrectBreakdown()
    {
        // Hoodie × 10, 18k stitches, 4 colors, digitizing, not urgent, 40% markup
        // Rule order: Garment → Stitches → Colors → Digitizing → Setup → BulkDiscount → Urgency → Markup
        //
        // Garment:      35 × 10          = 350    running: 350
        // Stitches:     18k/1k × 10 × 10 = 1800   running: 2150
        // Colors:       4 × 5            = 20     running: 2170
        // Digitizing:   one-time         = 30     running: 2200
        // Setup:        one-time         = 20     running: 2220
        // BulkDiscount: 10% of 2220      = -222   running: 1998
        // Urgency:      not urgent        = 0
        // Markup:       40% of 1998      = 799.2  running: 2797.2
        var ctx = PricingTestFactory.WorkedExampleContext();
        var result = _engine.Calculate(ctx);

        result.Adjustments.Should().Contain(a => a.Name == "Garment cost"       && a.Amount == 350m);
        result.Adjustments.Should().Contain(a => a.Name == "Stitch cost"        && a.Amount == 1800m);
        result.Adjustments.Should().Contain(a => a.Name == "Color complexity fee" && a.Amount == 20m);
        result.Adjustments.Should().Contain(a => a.Name == "Digitizing fee"     && a.Amount == 30m);
        result.Adjustments.Should().Contain(a => a.Name == "Setup fee"          && a.Amount == 20m);
        result.Adjustments.Should().Contain(a => a.Name == "Bulk discount"      && a.Amount == -222m);
        result.Adjustments.Should().Contain(a => a.Name == "Markup"             && a.Amount == 799.20m);

        result.Subtotal.Should().Be(2220m);       // costs + fees (not discount/markup)
        result.DiscountAmount.Should().Be(222m);
        result.MarkupAmount.Should().Be(799.20m);
        result.FinalPrice.Should().Be(2797.20m);
        result.PricePerItem.Should().Be(279.72m);
    }

    [Fact]
    public void Calculate_UrgentOrder_SurchargeAppliedAfterDiscount()
    {
        // garment=30, stitches=5k×10=50, colors=5, setup=20 → subtotal=105
        // qty=1 → no discount → running=105
        // urgency: 25% of 105 = 26.25 → running=131.25
        // markup: 40% of 131.25 = 52.5 → final=183.75
        var ctx = PricingTestFactory.DefaultContext(quantity: 1, isUrgent: true);
        var result = _engine.Calculate(ctx);

        result.Adjustments.Should().Contain(a => a.Name == "Urgency fee" && a.Amount == 26.25m);
        result.FinalPrice.Should().Be(183.75m);
    }

    [Fact]
    public void Calculate_UrgentBulkOrder_DiscountAppliedBeforeSurcharge()
    {
        // Urgency applies to the post-discount running total, not the raw base.
        // qty=10: discount first, then surcharge on discounted amount.
        var urgent = PricingTestFactory.DefaultContext(quantity: 10, isUrgent: true);
        var nonUrgent = PricingTestFactory.DefaultContext(quantity: 10, isUrgent: false);

        var urgentResult = _engine.Calculate(urgent);
        var baseResult   = _engine.Calculate(nonUrgent);

        urgentResult.FinalPrice.Should().BeGreaterThan(baseResult.FinalPrice);
        urgentResult.Adjustments.Should().Contain(a => a.Name == "Bulk discount");
        urgentResult.Adjustments.Should().Contain(a => a.Name == "Urgency fee");
    }

    [Fact]
    public void Calculate_SingleItemNoDiscounts_ReturnsExpected()
    {
        // garment=30, stitches=50, colors=5, setup=20 → subtotal=105
        // markup 40% × 105 = 42 → final=147
        var ctx = PricingTestFactory.DefaultContext(
            quantity: 1, stitchCount: 5000, colorCount: 1, garmentCost: 30m);

        var result = _engine.Calculate(ctx);

        result.Subtotal.Should().Be(105m);
        result.DiscountAmount.Should().Be(0m);
        result.FinalPrice.Should().Be(147m);
        result.PricePerItem.Should().Be(147m);
    }

    [Fact]
    public void Calculate_EnforcesMinimumOrderPrice()
    {
        var settings = PricingTestFactory.DefaultSettings();
        settings.PricePerThousandStitches = 0.01m;
        settings.DefaultMarkupPercentage  = 0m;
        settings.SetupFee                 = 0m;
        settings.ColorComplexityFeePerColor = 0m;
        settings.BulkDiscountEnabled      = false;
        settings.MinimumOrderPrice        = 50m;

        var ctx = PricingTestFactory.DefaultContext(
            quantity: 1, stitchCount: 100, colorCount: 1, garmentCost: 1m, settings: settings);

        _engine.Calculate(ctx).FinalPrice.Should().Be(50m);
    }

    [Fact]
    public void Calculate_AdjustmentsAreOrderedBySortOrder()
    {
        var result = _engine.Calculate(PricingTestFactory.WorkedExampleContext());
        result.Adjustments.Select(a => a.SortOrder).Should().BeInAscendingOrder();
    }

    [Fact]
    public void Calculate_PricePerItemEqualsFinalpriceDividedByQuantity()
    {
        var ctx = PricingTestFactory.DefaultContext(quantity: 7);
        var result = _engine.Calculate(ctx);
        decimal.Round(result.FinalPrice / 7, 2).Should().Be(result.PricePerItem);
    }

    [Fact]
    public void CreateDefault_CanBeReplacedWithCustomRules()
    {
        // DIP: engine accepts injected rules — verify it composes correctly with custom set
        var engine = new PricingEngine(
            rules: [new Domain.Pricing.Rules.GarmentCostRule()],
            finalizers: []);

        var ctx = PricingTestFactory.DefaultContext(quantity: 2, garmentCost: 50m);
        var result = engine.Calculate(ctx);

        result.FinalPrice.Should().Be(100m); // only garment cost, no markup
    }
}
