using FluentAssertions;
using StitchPrice.Domain.Enums;
using StitchPrice.Domain.Pricing.Rules;
using StitchPrice.UnitTests.Pricing;

namespace StitchPrice.UnitTests.Pricing.Rules;

public sealed class UrgencyFeeRuleTests
{
    private readonly UrgencyFeeRule _rule = new();

    [Fact]
    public void IsMatch_WhenUrgent_ReturnsTrue()
    {
        _rule.IsMatch(PricingTestFactory.DefaultContext(isUrgent: true)).Should().BeTrue();
    }

    [Fact]
    public void IsMatch_WhenNotUrgent_ReturnsFalse()
    {
        _rule.IsMatch(PricingTestFactory.DefaultContext(isUrgent: false)).Should().BeFalse();
    }

    [Fact]
    public void Apply_SurchargesRunningSubtotal()
    {
        // 25% of 100 = 25
        var ctx = PricingTestFactory.DefaultContext(isUrgent: true);
        var result = _rule.Apply(ctx, runningSubtotal: 100m);

        result.Amount.Should().Be(25m);
        result.Type.Should().Be(PricingAdjustmentType.Fee);
    }

    [Theory]
    [InlineData(1000, 250)]    // 25% of 1000
    [InlineData(2220, 555)]    // 25% of 2220
    [InlineData(1998, 499.5)]  // 25% of post-discount subtotal
    public void Apply_CalculatesCorrectSurcharge(decimal runningSubtotal, decimal expectedFee)
    {
        var ctx = PricingTestFactory.DefaultContext(isUrgent: true);
        _rule.Apply(ctx, runningSubtotal).Amount.Should().Be(expectedFee);
    }
}
