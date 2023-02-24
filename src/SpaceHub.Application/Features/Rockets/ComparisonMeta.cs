using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Rockets;

// TODO: add filters such as IsActive, FirstFlight <>
public record GetRocketsComparisonMetaQuery() : IRequest<Result<RocketsComparisonMetaVM>>;

internal class GetRocketsComparisonMetaHandler : IRequestHandler<GetRocketsComparisonMetaQuery, Result<RocketsComparisonMetaVM>>
{
    private readonly DbContext _db;

    public GetRocketsComparisonMetaHandler(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<RocketsComparisonMetaVM>> Handle(GetRocketsComparisonMetaQuery request, CancellationToken cancellationToken)
    {
        var rockets = await _db.Rockets.AsQueryable()
            .Select(x => new RocketShortInfoDto(x.ApiId, x.Family, x.Name, x.Variant))
            .ToListAsync();

        return new RocketsComparisonMetaVM()
        {
            TotalCount = rockets.Count,
            FamilyRocketsCountByName = rockets.Where(x => !string.IsNullOrEmpty(x.Family))
                .GroupBy(x => x.Family)
                .ToDictionary(k => k.Key, v => v.Count()),
            RocketIdsByName = rockets.Select(x => (Id: x.Id, FullName: CreateFullRocketName(x)))
                .GroupBy(x => x.FullName)
                .ToDictionary(k => k.Key, v => v.First().Id)
        };
    }

    private record RocketShortInfoDto(int Id, string Family, string Name, string Variant);

    private string CreateFullRocketName(RocketShortInfoDto rocketDto)
    {
        return rocketDto.Name + (!string.IsNullOrEmpty(rocketDto.Variant) ? " | " + rocketDto.Variant : string.Empty);
    }
}
