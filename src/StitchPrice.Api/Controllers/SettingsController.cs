using MediatR;
using Microsoft.AspNetCore.Mvc;
using StitchPrice.Application.Features.Settings.Commands;
using StitchPrice.Application.Features.Settings.DTOs;
using StitchPrice.Application.Features.Settings.Queries;

namespace StitchPrice.Api.Controllers;

[ApiController]
[Route("api/admin/pricing-settings")]
public sealed class SettingsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PricingSettingsDto>> Get(CancellationToken cancellationToken)
    {
        var settings = await sender.Send(new GetPricingSettingsQuery(), cancellationToken);
        return Ok(settings);
    }

    [HttpPut]
    public async Task<ActionResult<PricingSettingsDto>> Update(
        [FromBody] UpdatePricingSettingsCommand command,
        CancellationToken cancellationToken)
    {
        var settings = await sender.Send(command, cancellationToken);
        return Ok(settings);
    }
}
