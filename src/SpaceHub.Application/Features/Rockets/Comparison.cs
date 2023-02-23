using SpaceHub.Contracts.Utils;
using SpaceHub.Domain;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Rockets;

public record GetRocketsComparisonQuery(List<ComparisonGroup> ComparisonGroups) : IRequest<Result<RocketsComparisonVM>>;

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

        var groupsData = new Dictionary<Guid, RocketPropertiesFractionsVM>();

        foreach (var comparisonGroup in request.ComparisonGroups)
        {
            IEnumerable<Rocket> groupRockets = comparisonGroup switch
            {
                IndividualComparisonGroup individual => allRockets.Where(x => x.ApiId == individual.RocketId),
                FamilyComparisonGroup family => allRockets.Where(x => string.Equals(x.Family, family.FamilyName, StringComparison.OrdinalIgnoreCase)),
                AllComparisonGroup all => allRockets,
                _ => Enumerable.Empty<Rocket>()
            };

            groupsData.Add(comparisonGroup.Id, new()
            {
                Length = comparisonCalculator.CalculateFraction(ERocketComparisonProperty.Length, groupRockets),
                Diameter = comparisonCalculator.CalculateFraction(ERocketComparisonProperty.Diameter, groupRockets),
                LaunchCost = comparisonCalculator.CalculateFraction(ERocketComparisonProperty.LaunchCost, groupRockets),
                LiftoffMass = comparisonCalculator.CalculateFraction(ERocketComparisonProperty.LiftoffMass, groupRockets),
                LiftoffThrust = comparisonCalculator.CalculateFraction(ERocketComparisonProperty.LiftoffThrust, groupRockets),
                LeoCapacity = comparisonCalculator.CalculateFraction(ERocketComparisonProperty.LeoCapacity, groupRockets),
                GeoCapacity = comparisonCalculator.CalculateFraction(ERocketComparisonProperty.GeoCapacity, groupRockets),
                SuccessfulLaunches = comparisonCalculator.CalculateFraction(ERocketComparisonProperty.SuccessfulLaunches, groupRockets),
                LaunchSuccess = comparisonCalculator.CalculateFraction(ERocketComparisonProperty.LaunchSuccess, groupRockets),
            });
        }

        return new RocketsComparisonVM()
        {
            ComparisonGroupsData = groupsData,
        };
    }
}