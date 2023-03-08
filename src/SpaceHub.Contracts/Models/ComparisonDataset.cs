using SpaceHub.Contracts.Enums;
using System.Text.Json.Serialization;

namespace SpaceHub.Contracts.Models;

[JsonDerivedType(typeof(IndividualRocketsComparisonDataset), (int)ERocketComparisonDataset.Individual)]
[JsonDerivedType(typeof(FamilyRocketsComparisonDataset), (int)ERocketComparisonDataset.Family)]
[JsonDerivedType(typeof(AllRocketsComparisonDataset), (int)ERocketComparisonDataset.All)]
public abstract record RocketsComparisonDataset
{
    public abstract ERocketComparisonDataset Type { get; }
    public required Guid Id { get; init; }
}

public record IndividualRocketsComparisonDataset : RocketsComparisonDataset
{
    public required int RocketId { get; init; }
    public required string RocketName { get; init; }
    public override ERocketComparisonDataset Type => ERocketComparisonDataset.Individual;
}

public record FamilyRocketsComparisonDataset : RocketsComparisonDataset
{
    public required string FamilyName { get; init; }
    public override ERocketComparisonDataset Type => ERocketComparisonDataset.Family;
}

public record AllRocketsComparisonDataset : RocketsComparisonDataset
{
    public override ERocketComparisonDataset Type => ERocketComparisonDataset.All;
}
