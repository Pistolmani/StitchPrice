using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;

namespace StitchPrice.Application.Features.ProductProfiles.DTOs;

public sealed record ProductProfileDto(
    int Id,
    ProductType ProductType,
    decimal DefaultGarmentCost,
    decimal DefaultMarkupPercentage,
    decimal DifficultyMultiplier,
    bool IsActive)
{
    public static ProductProfileDto From(ProductPricingProfile p) => new(
        p.Id,
        p.ProductType,
        p.DefaultGarmentCost,
        p.DefaultMarkupPercentage,
        p.DifficultyMultiplier,
        p.IsActive);
}
