﻿using SpaceHub.Application.Interfaces;
using SpaceHub.Contracts.Enums;
using SpaceHub.Contracts.Models;
using SpaceHub.Domain;

namespace SpaceHub.Application.Features.Rockets;

public record GetRocketsComparisonQuery(IEnumerable<ComparisonDataset> Datasets) : IRequest<Result<RocketsComparisonVM>>;

public class GetRocketsComparisonQueryValidator : AbstractValidator<GetRocketsComparisonQuery>
{
    public GetRocketsComparisonQueryValidator()
    {
        RuleForEach(x => x.Datasets).SetValidator(new ComparisonDatasetValidator());
    }
}

internal sealed class GetRocketsComparisonHandler : IRequestHandler<GetRocketsComparisonQuery, Result<RocketsComparisonVM>>
{
    private readonly IDbContext _db;

    public GetRocketsComparisonHandler(IDbContext db)
    {
        _db = db;
    }

    public async Task<Result<RocketsComparisonVM>> Handle(GetRocketsComparisonQuery request, CancellationToken cancellationToken)
    {
        var allRockets = await _db.Rockets.AsQueryable().ToListAsync(cancellationToken);
        var comparisonCalculator = new RocketComparisonCalculator(allRockets);

        var groupsData = new Dictionary<Guid, IReadOnlyDictionary<ERocketComparisonProperty, RocketComparisonDatasetVM>>();
        var propertiesTypes = Enum.GetValues<ERocketComparisonProperty>();

        foreach (var dataset in request.Datasets)
        {
            var rocket = allRockets.FirstOrDefault(x => x.ApiId == dataset.RocketId);

            var groupData = new Dictionary<ERocketComparisonProperty, RocketComparisonDatasetVM>();
            foreach(var propertyType in propertiesTypes)
            {
                var (value, fraction, rank) = comparisonCalculator.CalculatePropertyRanking(propertyType, rocket);
                groupData.Add(propertyType, new RocketComparisonDatasetVM()
                {
                    Value = value,
                    Fraction = fraction,
                    Rank = rank,
                });
            }
            groupsData.Add(dataset.Id, groupData);
        }

        return new RocketsComparisonVM()
        {
            DatasetsById = groupsData,
        };
    }
}