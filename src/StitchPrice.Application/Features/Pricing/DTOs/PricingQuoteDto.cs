using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;

namespace StitchPrice.Application.Features.Pricing.DTOs;

public sealed record PricingQuoteDto(
    Guid QuoteId,
    ProductType ProductType,
    PlacementType PlacementType,
    FabricType FabricType,
    int Quantity,
    int StitchCount,
    int ColorCount,
    decimal GarmentCostPerItem,
    bool RequiresDigitizing,
    bool IsUrgent,
    string? Note,
    decimal Subtotal,
    decimal DiscountAmount,
    decimal MarkupAmount,
    decimal FinalPrice,
    decimal PricePerItem,
    decimal ProfitMarginPercentage,
    QuoteStatus Status,
    DateTime CreatedAtUtc,
    IReadOnlyList<PricingBreakdownItemDto> Breakdown)
{
    public static PricingQuoteDto From(PricingQuote quote) => new(
        QuoteId:               quote.Id,
        ProductType:           quote.ProductType,
        PlacementType:         quote.PlacementType,
        FabricType:            quote.FabricType,
        Quantity:              quote.Quantity,
        StitchCount:           quote.StitchCount,
        ColorCount:            quote.ColorCount,
        GarmentCostPerItem:    quote.GarmentCostPerItem,
        RequiresDigitizing:    quote.RequiresDigitizing,
        IsUrgent:              quote.IsUrgent,
        Note:                  quote.Note,
        Subtotal:              quote.Subtotal,
        DiscountAmount:        quote.DiscountAmount,
        MarkupAmount:          quote.MarkupAmount,
        FinalPrice:            quote.FinalPrice,
        PricePerItem:          quote.PricePerItem,
        ProfitMarginPercentage: quote.ProfitMarginPercentage,
        Status:                quote.Status,
        CreatedAtUtc:          quote.CreatedAtUtc,
        Breakdown:             quote.BreakdownItems
            .OrderBy(b => b.SortOrder)
            .Select(b => new PricingBreakdownItemDto(b.Name, b.Description, b.Amount, b.Type, b.SortOrder))
            .ToList());
}
