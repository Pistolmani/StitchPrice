using FluentValidation;
using MediatR;
using StitchPrice.Application.Features.ProductProfiles.DTOs;
using StitchPrice.Application.Interfaces;
using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;

namespace StitchPrice.Application.Features.ProductProfiles.Commands;

public sealed record CreateProductProfileCommand(
    ProductType ProductType,
    decimal DefaultGarmentCost,
    decimal DefaultMarkupPercentage,
    decimal DifficultyMultiplier,
    bool IsActive) : IRequest<ProductProfileDto>;

public sealed class CreateProductProfileValidator : AbstractValidator<CreateProductProfileCommand>
{
    public CreateProductProfileValidator()
    {
        RuleFor(x => x.ProductType).IsInEnum();
        RuleFor(x => x.DefaultGarmentCost).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DefaultMarkupPercentage).InclusiveBetween(0, 300);
        RuleFor(x => x.DifficultyMultiplier).GreaterThan(0);
    }
}

public sealed class CreateProductProfileHandler(IProductProfileRepository repo)
    : IRequestHandler<CreateProductProfileCommand, ProductProfileDto>
{
    public async Task<ProductProfileDto> Handle(
        CreateProductProfileCommand cmd,
        CancellationToken cancellationToken)
    {
        var profile = new ProductPricingProfile
        {
            ProductType              = cmd.ProductType,
            DefaultGarmentCost       = cmd.DefaultGarmentCost,
            DefaultMarkupPercentage  = cmd.DefaultMarkupPercentage,
            DifficultyMultiplier     = cmd.DifficultyMultiplier,
            IsActive                 = cmd.IsActive
        };

        await repo.AddAsync(profile, cancellationToken);
        return ProductProfileDto.From(profile);
    }
}
