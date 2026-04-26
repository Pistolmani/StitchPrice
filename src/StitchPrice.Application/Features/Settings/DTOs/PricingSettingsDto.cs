using StitchPrice.Domain.Entities;

namespace StitchPrice.Application.Features.Settings.DTOs;

public sealed record PricingSettingsDto(
    decimal PricePerThousandStitches,
    decimal SetupFee,
    decimal DigitizingFee,
    decimal UrgencyMultiplier,
    decimal DefaultMarkupPercentage,
    decimal MinimumOrderPrice,
    decimal ColorComplexityFeePerColor,
    bool BulkDiscountEnabled,
    DateTime UpdatedAtUtc)
{
    public static PricingSettingsDto From(PricingSettings s) => new(
        s.PricePerThousandStitches,
        s.SetupFee,
        s.DigitizingFee,
        s.UrgencyMultiplier,
        s.DefaultMarkupPercentage,
        s.MinimumOrderPrice,
        s.ColorComplexityFeePerColor,
        s.BulkDiscountEnabled,
        s.UpdatedAtUtc);
}
