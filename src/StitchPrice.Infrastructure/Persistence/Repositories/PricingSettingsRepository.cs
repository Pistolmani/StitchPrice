using Microsoft.EntityFrameworkCore;
using StitchPrice.Application.Interfaces;
using StitchPrice.Domain.Entities;

namespace StitchPrice.Infrastructure.Persistence.Repositories;

internal sealed class PricingSettingsRepository(StitchPriceDbContext db) : IPricingSettingsRepository
{
    public async Task<PricingSettings?> GetAsync(CancellationToken ct = default) =>
        await db.Settings.FirstOrDefaultAsync(ct);

    public async Task UpdateAsync(PricingSettings settings, CancellationToken ct = default)
    {
        db.Settings.Update(settings);
        await db.SaveChangesAsync(ct);
    }
}
