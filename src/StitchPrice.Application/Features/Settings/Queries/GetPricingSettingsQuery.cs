using MediatR;
using StitchPrice.Application.Features.Settings.DTOs;
using StitchPrice.Application.Interfaces;
using StitchPrice.Domain.Entities;

namespace StitchPrice.Application.Features.Settings.Queries;

public sealed record GetPricingSettingsQuery : IRequest<PricingSettingsDto>;

public sealed class GetPricingSettingsHandler(IPricingSettingsRepository repo)
    : IRequestHandler<GetPricingSettingsQuery, PricingSettingsDto>
{
    public async Task<PricingSettingsDto> Handle(
        GetPricingSettingsQuery query,
        CancellationToken cancellationToken)
    {
        var settings = await repo.GetAsync(cancellationToken) ?? PricingSettings.Default();
        return PricingSettingsDto.From(settings);
    }
}
