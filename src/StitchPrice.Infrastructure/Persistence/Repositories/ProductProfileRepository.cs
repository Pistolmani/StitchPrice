using Microsoft.EntityFrameworkCore;
using StitchPrice.Application.Interfaces;
using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;

namespace StitchPrice.Infrastructure.Persistence.Repositories;

internal sealed class ProductProfileRepository(StitchPriceDbContext db) : IProductProfileRepository
{
    public async Task<IReadOnlyList<ProductPricingProfile>> GetAllAsync(CancellationToken ct = default) =>
        await db.ProductProfiles.AsNoTracking().OrderBy(p => p.ProductType).ToListAsync(ct);

    public async Task<ProductPricingProfile?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await db.ProductProfiles.FindAsync([id], ct);

    public async Task<ProductPricingProfile?> FindByProductTypeAsync(
        ProductType type, CancellationToken ct = default) =>
        await db.ProductProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductType == type && p.IsActive, ct);

    public async Task AddAsync(ProductPricingProfile profile, CancellationToken ct = default)
    {
        await db.ProductProfiles.AddAsync(profile, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ProductPricingProfile profile, CancellationToken ct = default)
    {
        db.ProductProfiles.Update(profile);
        await db.SaveChangesAsync(ct);
    }
}
