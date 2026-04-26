using FluentAssertions;
using StitchPrice.Domain.Enums;
using StitchPrice.Domain.Pricing.Rules;
using StitchPrice.UnitTests.Pricing;

namespace StitchPrice.UnitTests.Pricing.Rules;

public sealed class BulkDiscountRuleTests
{
    private readonly BulkDiscountRule _rule = new();

    [Theory]
    [InlineData(1, 0)]
    [InlineData(4, 0)]
    [InlineData(5, 0.05)]
    [InlineData(9, 0.05)]
    [InlineData(10, 0.10)]
    [InlineData(24, 0.10)]
    [InlineData(25, 0.15)]
    [InlineData(49, 0.15)]
    [InlineData(50, 0.20)]
    [InlineData(100, 0.20)]
    public void GetDiscountRate_ReturnsCorrectTier(int quantity, decimal expectedRate)
    {
        BulkDiscountRule.GetDiscountRate(quantity).Should().Be(expectedRate);
    }

    [Fact]
    public void IsMatch_WhenQualifyingQuantity_ReturnsTrue()
    {
        _rule.IsMatch(PricingTestFactory.DefaultContext(quantity: 10)).Should().BeTrue();
    }

    [Fact]
    public void IsMatch_WhenBelowThreshold_ReturnsFalse()
    {
        _rule.IsMatch(PricingTestFactory.DefaultContext(quantity: 3)).Should().BeFalse();
    }

    [Fact]
    public void IsMatch_WhenBulkDiscountDisabled_ReturnsFalse()
    {
        var settings = PricingTestFactory.DefaultSettings();
        settings.BulkDiscountEnabled = false;
        _rule.IsMatch(PricingTestFactory.DefaultContext(quantity: 50, settings: settings)).Should().BeFalse();
    }

    [Fact]
    public void Apply_DiscountsRunningSubtotal()
    {
        // 10% of 1000 = 100
        var ctx = PricingTestFactory.DefaultContext(quantity: 10);
        var result = _rule.Apply(ctx, runningSubtotal: 1000m);

        result.Amount.Should().Be(-100m);
        result.Type.Should().Be(PricingAdjustmentType.Discount);
    }

    [Fact]
    public void Apply_ReturnsNegativeAmount()
    {
        var ctx = PricingTestFactory.DefaultContext(quantity: 10);
        _rule.Apply(ctx, runningSubtotal: 500m).Amount.Should().BeNegative();
    }
}
