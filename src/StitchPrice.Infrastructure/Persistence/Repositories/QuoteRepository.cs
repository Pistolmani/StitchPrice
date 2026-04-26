using Microsoft.EntityFrameworkCore;
using StitchPrice.Application.Interfaces;
using StitchPrice.Domain.Entities;

namespace StitchPrice.Infrastructure.Persistence.Repositories;

internal sealed class QuoteRepository(StitchPriceDbContext db) : IQuoteRepository
{
    public async Task<PricingQuote?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await db.Quotes
            .Include(q => q.BreakdownItems)
            .FirstOrDefaultAsync(q => q.Id == id, ct);

    public async Task<IReadOnlyList<PricingQuote>> GetAllAsync(
        int page, int pageSize, CancellationToken ct = default) =>
        await db.Quotes
            .AsNoTracking()
            .Include(q => q.BreakdownItems)
            .OrderByDescending(q => q.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task AddAsync(PricingQuote quote, CancellationToken ct = default)
    {
        await db.Quotes.AddAsync(quote, ct);
        await db.SaveChangesAsync(ct);
    }
}
