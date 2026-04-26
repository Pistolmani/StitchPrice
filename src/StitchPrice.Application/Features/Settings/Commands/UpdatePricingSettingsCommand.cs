using FluentValidation;
using MediatR;
using StitchPrice.Application.Features.Settings.DTOs;
using StitchPrice.Application.Interfaces;
using StitchPrice.Domain.Entities;

namespace StitchPrice.Application.Features.Settings.Commands;

public sealed record UpdatePricingSettingsCommand(
    decimal PricePerThousandStitches,
    decimal SetupFee,
    decimal DigitizingFee,
    decimal UrgencyMultiplier,
    decimal DefaultMarkupPercentage,
    decimal MinimumOrderPrice,
    decimal ColorComplexityFeePerColor,
    bool BulkDiscountEnabled) : IRequest<PricingSettingsDto>;

public sealed class UpdatePricingSettingsValidator : AbstractValidator<UpdatePricingSettingsCommand>
{
    public UpdatePricingSettingsValidator()
    {
        RuleFor(x => x.PricePerThousandStitches)
            .GreaterThan(0).WithMessage("Price per thousand stitches must be greater than 0.");

        RuleFor(x => x.SetupFee)
            .GreaterThanOrEqualTo(0).WithMessage("Setup fee cannot be negative.");

        RuleFor(x => x.DigitizingFee)
            .GreaterThanOrEqualTo(0).WithMessage("Digitizing fee cannot be negative.");

        RuleFor(x => x.UrgencyMultiplier)
            .GreaterThanOrEqualTo(1).WithMessage("Urgency multiplier must be at least 1.");

        RuleFor(x => x.DefaultMarkupPercentage)
            .InclusiveBetween(0, 300).WithMessage("Default markup must be between 0 and 300.");

        RuleFor(x => x.MinimumOrderPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum order price cannot be negative.");

        RuleFor(x => x.ColorComplexityFeePerColor)
            .GreaterThanOrEqualTo(0).WithMessage("Color complexity fee cannot be negative.");
    }
}

public sealed class UpdatePricingSettingsHandler(IPricingSettingsRepository repo)
    : IRequestHandler<UpdatePricingSettingsCommand, PricingSettingsDto>
{
    public async Task<PricingSettingsDto> Handle(
        UpdatePricingSettingsCommand cmd,
        CancellationToken cancellationToken)
    {
        var settings = await repo.GetAsync(cancellationToken) ?? PricingSettings.Default();

        settings.PricePerThousandStitches  = cmd.PricePerThousandStitches;
        settings.SetupFee                  = cmd.SetupFee;
        settings.DigitizingFee             = cmd.DigitizingFee;
        settings.UrgencyMultiplier         = cmd.UrgencyMultiplier;
        settings.DefaultMarkupPercentage   = cmd.DefaultMarkupPercentage;
        settings.MinimumOrderPrice         = cmd.MinimumOrderPrice;
        settings.ColorComplexityFeePerColor = cmd.ColorComplexityFeePerColor;
        settings.BulkDiscountEnabled       = cmd.BulkDiscountEnabled;
        settings.UpdatedAtUtc              = DateTime.UtcNow;

        await repo.UpdateAsync(settings, cancellationToken);

        return PricingSettingsDto.From(settings);
    }
}
