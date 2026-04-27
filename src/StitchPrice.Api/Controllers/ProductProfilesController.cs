using MediatR;
using Microsoft.AspNetCore.Mvc;
using StitchPrice.Application.Features.ProductProfiles.Commands;
using StitchPrice.Application.Features.ProductProfiles.DTOs;
using StitchPrice.Application.Features.ProductProfiles.Queries;

namespace StitchPrice.Api.Controllers;

[ApiController]
[Route("api/admin/product-profiles")]
public sealed class ProductProfilesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductProfileDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var profiles = await sender.Send(new GetProductProfilesQuery(), cancellationToken);
        return Ok(profiles);
    }

    [HttpPost]
    public async Task<ActionResult<ProductProfileDto>> Create(
        [FromBody] CreateProductProfileCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id = profile.Id }, profile);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductProfileDto>> Update(
        int id,
        [FromBody] UpdateProductProfileCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await sender.Send(command with { Id = id }, cancellationToken);
        return Ok(profile);
    }
}
