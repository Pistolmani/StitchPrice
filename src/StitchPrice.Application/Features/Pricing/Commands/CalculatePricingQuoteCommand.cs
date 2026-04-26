using MediatR;
using StitchPrice.Application.Features.Pricing.DTOs;
using StitchPrice.Domain.Enums;

namespace StitchPrice.Application.Features.Pricing.Commands;

public sealed record CalculatePricingQuoteCommand(
    ProductType ProductType,
    PlacementType PlacementType,
    FabricType FabricType,
    int Quantity,
    int StitchCount,
    int ColorCount,
    decimal GarmentCostPerItem,
    bool RequiresDigitizing,
    bool IsUrgent,
    string? Note) : IRequest<PricingQuoteDto>;
