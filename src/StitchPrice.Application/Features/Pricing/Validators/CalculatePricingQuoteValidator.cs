using FluentValidation;
using StitchPrice.Application.Features.Pricing.Commands;

namespace StitchPrice.Application.Features.Pricing.Validators;

public sealed class CalculatePricingQuoteValidator : AbstractValidator<CalculatePricingQuoteCommand>
{
    public CalculatePricingQuoteValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.StitchCount)
            .GreaterThan(0).WithMessage("Stitch count must be greater than 0.");

        RuleFor(x => x.ColorCount)
            .InclusiveBetween(1, 15).WithMessage("Color count must be between 1 and 15.");

        RuleFor(x => x.GarmentCostPerItem)
            .GreaterThanOrEqualTo(0).WithMessage("Garment cost cannot be negative.");

        RuleFor(x => x.ProductType)
            .IsInEnum().WithMessage("Product type is required.");

        RuleFor(x => x.PlacementType)
            .IsInEnum().WithMessage("Placement type is required.");

        RuleFor(x => x.FabricType)
            .IsInEnum().WithMessage("Fabric type is required.");

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Note cannot exceed 500 characters.")
            .When(x => x.Note is not null);
    }
}
