using FluentAssertions;
using StitchPrice.Domain.Enums;
using StitchPrice.Domain.Pricing.Rules;
using StitchPrice.UnitTests.Pricing;

namespace StitchPrice.UnitTests.Pricing.Rules;

public sealed class StitchCountRuleTests
{
    private readonly StitchCountRule _rule = new();

    [Theory]
    [InlineData(1000, 1, 10, 10)]
    [InlineData(5000, 1, 10, 50)]
    [InlineData(18000, 10, 10, 1800)]
    public void Apply_CalculatesCorrectly(int stitches, int quantity, decimal pricePerK, decimal expected)
    {
        var settings = PricingTestFactory.DefaultSettings();
        settings.PricePerThousandStitches = pricePerK;
        var ctx = PricingTestFactory.DefaultContext(quantity: quantity, stitchCount: stitches, settings: settings);

        _rule.Apply(ctx, runningSubtotal: 0m).Amount.Should().Be(expected);
    }

    [Fact]
    public void Apply_HandlesNonRoundStitchCount()
    {
        // 1500 stitches × 10/k × 1 = 15
        var ctx = PricingTestFactory.DefaultContext(stitchCount: 1500);
        _rule.Apply(ctx, runningSubtotal: 0m).Amount.Should().Be(15m);
    }

    [Fact]
    public void Apply_ReturnsTypeCost()
    {
        var ctx = PricingTestFactory.DefaultContext();
        _rule.Apply(ctx, runningSubtotal: 0m).Type.Should().Be(PricingAdjustmentType.Cost);
    }
}
