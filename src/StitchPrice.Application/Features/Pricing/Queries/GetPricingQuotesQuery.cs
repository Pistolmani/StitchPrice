using MediatR;
using StitchPrice.Application.Features.Pricing.DTOs;
using StitchPrice.Application.Interfaces;

namespace StitchPrice.Application.Features.Pricing.Queries;

public sealed record GetPricingQuotesQuery(int Page = 1, int PageSize = 20)
    : IRequest<IReadOnlyList<PricingQuoteDto>>;

public sealed class GetPricingQuotesHandler(IQuoteRepository repo)
    : IRequestHandler<GetPricingQuotesQuery, IReadOnlyList<PricingQuoteDto>>
{
    public async Task<IReadOnlyList<PricingQuoteDto>> Handle(
        GetPricingQuotesQuery query,
        CancellationToken cancellationToken)
    {
        var quotes = await repo.GetAllAsync(query.Page, query.PageSize, cancellationToken);
        return quotes.Select(PricingQuoteDto.From).ToList();
    }
}
