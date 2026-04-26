using StitchPrice.Domain.Entities;

namespace StitchPrice.Application.Interfaces;

public interface IPricingSettingsRepository
{
    /// <summary>Returns the singleton settings row, or null if not yet seeded.</summary>
    Task<PricingSettings?> GetAsync(CancellationToken ct = default);
    Task UpdateAsync(PricingSettings settings, CancellationToken ct = default);
}
