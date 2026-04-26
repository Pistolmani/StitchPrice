using StitchPrice.Domain.Entities;
using StitchPrice.Domain.Enums;

namespace StitchPrice.Application.Interfaces;

public interface IProductProfileRepository
{
    Task<IReadOnlyList<ProductPricingProfile>> GetAllAsync(CancellationToken ct = default);
    Task<ProductPricingProfile?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ProductPricingProfile?> FindByProductTypeAsync(ProductType type, CancellationToken ct = default);
    Task AddAsync(ProductPricingProfile profile, CancellationToken ct = default);
    Task UpdateAsync(ProductPricingProfile profile, CancellationToken ct = default);
}
