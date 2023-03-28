using SpaceHub.Contracts.Enums;
using SpaceHub.Domain;
using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Rockets;

public record GetRocketsComparisonMetaQuery(int TopValuesCount) : IRequest<Result<RocketsComparisonMetaVM>>;

public class GetRocketsComparisonMetaQueryValidator : AbstractValidator<GetRocketsComparisonMetaQuery>
{
    public GetRocketsComparisonMetaQueryValidator()
    {
        RuleFor(x => x.TopValuesCount).GreaterThan(0);
    }
}

internal class GetRocketsComparisonMetaHandler : IRequestHandler<GetRocketsComparisonMetaQuery, Result<RocketsComparisonMetaVM>>
{
    private readonly DbContext _db;

    public GetRocketsComparisonMetaHandler(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<RocketsComparisonMetaVM>> Handle(GetRocketsComparisonMetaQuery request, CancellationToken cancellationToken)
    {
        var allRockets = (await _db.Rockets.AsQueryable().ToListAsync()).Select(x => x.ToDomainModel());
        var rockets = allRockets.Select(x => new RocketShortInfoDto(x.ApiId, x.Family, x.Name, x.Variant)).ToList();

        var comparisonCalculator = new RocketComparisonCalculator(allRockets);

        return new RocketsComparisonMetaVM()
        {
            TotalCount = rockets.Count,
            RocketIdsByName = rockets.Select(x => (Id: x.Id, FullName: CreateFullRocketName(x)))
                .GroupBy(x => x.FullName)
                .ToDictionary(k => k.Key, v => v.First().Id),
            TopValuesByPropertyType = GetTopValuesByPropertyType(comparisonCalculator, request.TopValuesCount)
        };
    }

    private IReadOnlyDictionary<ERocketComparisonProperty, IReadOnlyList<RocketPropertyValueVM>> GetTopValuesByPropertyType(
        RocketComparisonCalculator comparisonCalculator, int count)
    {
        var topValues = comparisonCalculator.GetTopValues(count);
        var result = new Dictionary<ERocketComparisonProperty, IReadOnlyList<RocketPropertyValueVM>>();
        foreach (var (key, value) in topValues)
        {
            var propertyValues = value.Select(x => new RocketPropertyValueVM()
            {
                Value = x.Value,
                RocketsNames = x.Names
            }).ToList();
            result.Add(key, propertyValues);
        }
        return result;
    }

    private record RocketShortInfoDto(int Id, string Family, string Name, string Variant);

    private static string CreateFullRocketName(RocketShortInfoDto rocketDto)
    {
        return rocketDto.Name + (!string.IsNullOrEmpty(rocketDto.Variant) ? " | " + rocketDto.Variant : string.Empty);
    }
}
