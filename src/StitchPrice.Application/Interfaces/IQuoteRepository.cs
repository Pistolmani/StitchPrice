using StitchPrice.Domain.Entities;

namespace StitchPrice.Application.Interfaces;

public interface IQuoteRepository
{
    Task<PricingQuote?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<PricingQuote>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(PricingQuote quote, CancellationToken ct = default);
}
