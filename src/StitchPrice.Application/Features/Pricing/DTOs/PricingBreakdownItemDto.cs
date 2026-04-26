using StitchPrice.Domain.Enums;

namespace StitchPrice.Application.Features.Pricing.DTOs;

public sealed record PricingBreakdownItemDto(
    string Name,
    string Description,
    decimal Amount,
    PricingAdjustmentType Type,
    int SortOrder);
