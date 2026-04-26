using MediatR;
using StitchPrice.Application.Features.Pricing.DTOs;
using StitchPrice.Application.Interfaces;
using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;
using StitchPrice.Domain.Pricing;

namespace StitchPrice.Application.Features.Pricing.Commands;

public sealed class CalculatePricingQuoteHandler(
    PricingEngine engine,
    IQuoteRepository quoteRepo,
    IPricingSettingsRepository settingsRepo,
    IProductProfileRepository profileRepo)
    : IRequestHandler<CalculatePricingQuoteCommand, PricingQuoteDto>
{
    public async Task<PricingQuoteDto> Handle(
        CalculatePricingQuoteCommand cmd,
        CancellationToken cancellationToken)
    {
        var settings = await settingsRepo.GetAsync(cancellationToken)
                       ?? PricingSettings.Default();

        var profile = await profileRepo.FindByProductTypeAsync(cmd.ProductType, cancellationToken);

        var context = new PricingContext
        {
            ProductType        = cmd.ProductType,
            PlacementType      = cmd.PlacementType,
            FabricType         = cmd.FabricType,
            Quantity           = cmd.Quantity,
            StitchCount        = cmd.StitchCount,
            ColorCount         = cmd.ColorCount,
            GarmentCostPerItem = cmd.GarmentCostPerItem,
            RequiresDigitizing = cmd.RequiresDigitizing,
            IsUrgent           = cmd.IsUrgent,
            Settings           = settings,
            ProductProfile     = profile
        };

        var result = engine.Calculate(context);

        var quote = new PricingQuote
        {
            Id                     = Guid.NewGuid(),
            ProductType            = cmd.ProductType,
            PlacementType          = cmd.PlacementType,
            FabricType             = cmd.FabricType,
            Quantity               = cmd.Quantity,
            StitchCount            = cmd.StitchCount,
            ColorCount             = cmd.ColorCount,
            GarmentCostPerItem     = cmd.GarmentCostPerItem,
            RequiresDigitizing     = cmd.RequiresDigitizing,
            IsUrgent               = cmd.IsUrgent,
            Note                   = cmd.Note,
            SetupFee               = settings.SetupFee,
            DigitizingFee          = cmd.RequiresDigitizing ? settings.DigitizingFee : 0m,
            Subtotal               = result.Subtotal,
            DiscountAmount         = result.DiscountAmount,
            MarkupAmount           = result.MarkupAmount,
            FinalPrice             = result.FinalPrice,
            PricePerItem           = result.PricePerItem,
            ProfitMarginPercentage = result.ProfitMarginPercentage,
            Status                 = QuoteStatus.Calculated,
            CreatedAtUtc           = DateTime.UtcNow,
            BreakdownItems         = result.Adjustments
                .Select(a => new PricingBreakdownItem
                {
                    Name        = a.Name,
                    Description = a.Description,
                    Amount      = a.Amount,
                    Type        = a.Type,
                    SortOrder   = a.SortOrder
                })
                .ToList()
        };

        await quoteRepo.AddAsync(quote, cancellationToken);

        return PricingQuoteDto.From(quote);
    }
}
