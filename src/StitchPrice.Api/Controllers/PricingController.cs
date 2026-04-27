using MediatR;
using Microsoft.AspNetCore.Mvc;
using StitchPrice.Application.Features.Pricing.Commands;
using StitchPrice.Application.Features.Pricing.DTOs;
using StitchPrice.Application.Features.Pricing.Queries;

namespace StitchPrice.Api.Controllers;

[ApiController]
[Route("api/pricing")]
public sealed class PricingController(ISender sender) : ControllerBase
{
    [HttpPost("calculate")]
    public async Task<ActionResult<PricingQuoteDto>> Calculate(
        [FromBody] CalculatePricingQuoteCommand command,
        CancellationToken cancellationToken)
    {
        var quote = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = quote.QuoteId }, quote);
    }

    [HttpGet("quotes")]
    public async Task<ActionResult<IReadOnlyList<PricingQuoteDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var quotes = await sender.Send(new GetPricingQuotesQuery(page, pageSize), cancellationToken);
        return Ok(quotes);
    }

    [HttpGet("quotes/{id:guid}")]
    public async Task<ActionResult<PricingQuoteDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var quote = await sender.Send(new GetPricingQuoteByIdQuery(id), cancellationToken);
        return Ok(quote);
    }
}
