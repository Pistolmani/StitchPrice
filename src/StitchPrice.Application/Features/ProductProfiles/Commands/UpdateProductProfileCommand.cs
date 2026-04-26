using FluentValidation;
using MediatR;
using StitchPrice.Application.Common.Exceptions;
using StitchPrice.Application.Features.ProductProfiles.DTOs;
using StitchPrice.Application.Interfaces;

namespace StitchPrice.Application.Features.ProductProfiles.Commands;

public sealed record UpdateProductProfileCommand(
    int Id,
    decimal DefaultGarmentCost,
    decimal DefaultMarkupPercentage,
    decimal DifficultyMultiplier,
    bool IsActive) : IRequest<ProductProfileDto>;

public sealed class UpdateProductProfileValidator : AbstractValidator<UpdateProductProfileCommand>
{
    public UpdateProductProfileValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.DefaultGarmentCost).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DefaultMarkupPercentage).InclusiveBetween(0, 300);
        RuleFor(x => x.DifficultyMultiplier).GreaterThan(0);
    }
}

public sealed class UpdateProductProfileHandler(IProductProfileRepository repo)
    : IRequestHandler<UpdateProductProfileCommand, ProductProfileDto>
{
    public async Task<ProductProfileDto> Handle(
        UpdateProductProfileCommand cmd,
        CancellationToken cancellationToken)
    {
        var profile = await repo.GetByIdAsync(cmd.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.ProductPricingProfile), cmd.Id);

        profile.DefaultGarmentCost      = cmd.DefaultGarmentCost;
        profile.DefaultMarkupPercentage = cmd.DefaultMarkupPercentage;
        profile.DifficultyMultiplier    = cmd.DifficultyMultiplier;
        profile.IsActive                = cmd.IsActive;

        await repo.UpdateAsync(profile, cancellationToken);
        return ProductProfileDto.From(profile);
    }
}
