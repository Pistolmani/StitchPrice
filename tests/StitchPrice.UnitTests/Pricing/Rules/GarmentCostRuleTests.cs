using FluentAssertions;
using StitchPrice.Domain.Enums;
using StitchPrice.Domain.Pricing.Rules;
using StitchPrice.UnitTests.Pricing;

namespace StitchPrice.UnitTests.Pricing.Rules;

public sealed class GarmentCostRuleTests
{
    private readonly GarmentCostRule _rule = new();

    [Fact]
    public void IsMatch_AlwaysTrue()
    {
        _rule.IsMatch(PricingTestFactory.DefaultContext()).Should().BeTrue();
    }

    [Theory]
    [InlineData(1, 30, 30)]
    [InlineData(10, 35, 350)]
    [InlineData(50, 20, 1000)]
    public void Apply_ReturnsQuantityTimesGarmentCost(int quantity, decimal cost, decimal expected)
    {
        var ctx = PricingTestFactory.DefaultContext(quantity: quantity, garmentCost: cost);
        var result = _rule.Apply(ctx, runningSubtotal: 0m);

        result.Amount.Should().Be(expected);
        result.Type.Should().Be(PricingAdjustmentType.Cost);
    }
}
