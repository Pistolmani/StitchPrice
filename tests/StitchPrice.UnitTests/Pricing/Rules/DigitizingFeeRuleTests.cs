using FluentAssertions;
using StitchPrice.Domain.Enums;
using StitchPrice.Domain.Pricing.Rules;
using StitchPrice.UnitTests.Pricing;

namespace StitchPrice.UnitTests.Pricing.Rules;

public sealed class DigitizingFeeRuleTests
{
    private readonly DigitizingFeeRule _rule = new();

    [Fact]
    public void IsMatch_WhenDigitizingRequired_ReturnsTrue()
    {
        _rule.IsMatch(PricingTestFactory.DefaultContext(requiresDigitizing: true)).Should().BeTrue();
    }

    [Fact]
    public void IsMatch_WhenDigitizingNotRequired_ReturnsFalse()
    {
        _rule.IsMatch(PricingTestFactory.DefaultContext(requiresDigitizing: false)).Should().BeFalse();
    }

    [Fact]
    public void Apply_ReturnsSettingsDigitizingFee()
    {
        var ctx = PricingTestFactory.DefaultContext(requiresDigitizing: true);
        var result = _rule.Apply(ctx, runningSubtotal: 0m);

        result.Amount.Should().Be(ctx.Settings.DigitizingFee);
        result.Type.Should().Be(PricingAdjustmentType.Fee);
    }
}
