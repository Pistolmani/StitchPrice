using MediatR;
using StitchPrice.Application.Features.ProductProfiles.DTOs;
using StitchPrice.Application.Interfaces;

namespace StitchPrice.Application.Features.ProductProfiles.Queries;

public sealed record GetProductProfilesQuery : IRequest<IReadOnlyList<ProductProfileDto>>;

public sealed class GetProductProfilesHandler(IProductProfileRepository repo)
    : IRequestHandler<GetProductProfilesQuery, IReadOnlyList<ProductProfileDto>>
{
    public async Task<IReadOnlyList<ProductProfileDto>> Handle(
        GetProductProfilesQuery query,
        CancellationToken cancellationToken)
    {
        var profiles = await repo.GetAllAsync(cancellationToken);
        return profiles.Select(ProductProfileDto.From).ToList();
    }
}
