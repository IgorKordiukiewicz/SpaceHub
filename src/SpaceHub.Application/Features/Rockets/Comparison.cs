using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.Models;
using SpaceHub.Domain;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Rockets;

public record GetRocketsComparisonQuery(IEnumerable<ComparisonGroup> ComparisonGroups) : IRequest<Result<RocketsComparisonVM>>;

internal class GetRocketsComparisonHandler : IRequestHandler<GetRocketsComparisonQuery, Result<RocketsComparisonVM>>
{
    private readonly DbContext _db;

    public GetRocketsComparisonHandler(DbContext db)
    {
        _db = db;
    }

    public async Task<Result<RocketsComparisonVM>> Handle(GetRocketsComparisonQuery request, CancellationToken cancellationToken)
    {
        var allRockets = (await _db.Rockets.AsQueryable().ToListAsync()).Select(x => x.ToDomainModel());
        var comparisonCalculator = new RocketComparisonCalculator(allRockets);

        var groupsData = new Dictionary<Guid, IReadOnlyDictionary<ERocketComparisonProperty, double>>();
        var propertiesTypes = Enum.GetValues<ERocketComparisonProperty>();

        foreach (var comparisonGroup in request.ComparisonGroups)
        {
            var groupRockets = comparisonGroup switch
            {
                IndividualComparisonGroup individual => allRockets.Where(x => x.ApiId == individual.RocketId).ToList(),
                FamilyComparisonGroup family => allRockets.Where(x => string.Equals(x.Family, family.FamilyName, StringComparison.OrdinalIgnoreCase)).ToList(),
                AllComparisonGroup all => allRockets.ToList(),
                _ => new List<Rocket>()
            };

            var groupData = new Dictionary<ERocketComparisonProperty, double>();
            foreach(var propertyType in propertiesTypes)
            {
                groupData.Add(propertyType, comparisonCalculator.CalculateFraction(propertyType, groupRockets));
            }
            groupsData.Add(comparisonGroup.Id, groupData);
        }

        return new RocketsComparisonVM()
        {
            ComparisonGroupsData = groupsData,
        };
    }
}