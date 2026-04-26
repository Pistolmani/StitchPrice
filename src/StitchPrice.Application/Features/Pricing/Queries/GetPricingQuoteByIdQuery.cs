using MediatR;
using StitchPrice.Application.Common.Exceptions;
using StitchPrice.Application.Features.Pricing.DTOs;
using StitchPrice.Application.Interfaces;

namespace StitchPrice.Application.Features.Pricing.Queries;

public sealed record GetPricingQuoteByIdQuery(Guid Id) : IRequest<PricingQuoteDto>;

public sealed class GetPricingQuoteByIdHandler(IQuoteRepository repo)
    : IRequestHandler<GetPricingQuoteByIdQuery, PricingQuoteDto>
{
    public async Task<PricingQuoteDto> Handle(
        GetPricingQuoteByIdQuery query,
        CancellationToken cancellationToken)
    {
        var quote = await repo.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.PricingQuote), query.Id);

        return PricingQuoteDto.From(quote);
    }
}
