using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.Models;
using SpaceHub.Domain;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Data;

namespace SpaceHub.Application.Features.Rockets;

public record GetRocketsComparisonQuery(IEnumerable<RocketsComparisonDataset> ComparisonGroups) : IRequest<Result<RocketsComparisonVM>>;

public class GetRocketsComparisonQueryValidator : AbstractValidator<GetRocketsComparisonQuery>
{
    public GetRocketsComparisonQueryValidator()
    {
        RuleForEach(x => x.ComparisonGroups).SetValidator(new RocketComparisonDatasetValidator());
    }
}

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

        var groupsData = new Dictionary<Guid, IReadOnlyDictionary<ERocketComparisonProperty, RocketComparisonDatasetVM>>();
        var propertiesTypes = Enum.GetValues<ERocketComparisonProperty>();

        foreach (var comparisonGroup in request.ComparisonGroups)
        {
            var groupRockets = comparisonGroup switch
            {
                IndividualRocketsComparisonDataset individual => allRockets.Where(x => x.ApiId == individual.RocketId).ToList(),
                FamilyRocketsComparisonDataset family => allRockets.Where(x => string.Equals(x.Family, family.FamilyName, StringComparison.OrdinalIgnoreCase)).ToList(),
                AllRocketsComparisonDataset all => allRockets.ToList(),
                _ => new List<Rocket>()
            };

            var groupData = new Dictionary<ERocketComparisonProperty, RocketComparisonDatasetVM>();
            foreach(var propertyType in propertiesTypes)
            {
                var (value, fraction, rank) = comparisonCalculator.CalculatePropertyRanking(propertyType, groupRockets);
                groupData.Add(propertyType, new RocketComparisonDatasetVM()
                {
                    Value = value,
                    Fraction = fraction,
                    Rank = rank,
                });
            }
            groupsData.Add(comparisonGroup.Id, groupData);
        }

        return new RocketsComparisonVM()
        {
            DatasetsById = groupsData,
        };
    }
}