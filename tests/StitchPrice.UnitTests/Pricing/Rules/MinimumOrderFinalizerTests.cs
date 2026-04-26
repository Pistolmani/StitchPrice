using FluentAssertions;
using StitchPrice.Domain.Enums;
using StitchPrice.Domain.Pricing.Rules;
using StitchPrice.UnitTests.Pricing;

namespace StitchPrice.UnitTests.Pricing.Rules;

public sealed class MinimumOrderFinalizerTests
{
    private readonly MinimumOrderFinalizer _finalizer = new();

    [Fact]
    public void Apply_WhenBelowMinimum_ReturnsAdjustmentForDifference()
    {
        var ctx = PricingTestFactory.DefaultContext(); // minimum = 50
        var result = _finalizer.Apply(totalAfterRules: 10m, ctx);

        result.Should().NotBeNull();
        result!.Amount.Should().Be(40m); // 50 - 10
        result.Type.Should().Be(PricingAdjustmentType.Fee);
    }

    [Fact]
    public void Apply_WhenAtMinimum_ReturnsNull()
    {
        var ctx = PricingTestFactory.DefaultContext();
        _finalizer.Apply(totalAfterRules: 50m, ctx).Should().BeNull();
    }

    [Fact]
    public void Apply_WhenAboveMinimum_ReturnsNull()
    {
        var ctx = PricingTestFactory.DefaultContext();
        _finalizer.Apply(totalAfterRules: 200m, ctx).Should().BeNull();
    }
}
