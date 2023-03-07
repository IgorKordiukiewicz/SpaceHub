using SpaceHub.Contracts.Enums;

namespace SpaceHub.Contracts.ViewModels;

public record RocketsComparisonMetaVM
{
    public required int TotalCount { get; init; }
    public required IReadOnlyDictionary<string, int> FamilyRocketsCountByName { get; init; }
    public required IReadOnlyDictionary<string, int> RocketIdsByName { get; init; }
    public required IReadOnlyDictionary<ERocketComparisonProperty, IReadOnlyList<RocketPropertyValueVM>> TopValuesByPropertyType { get; init; }
}

public record RocketPropertyValueVM
{
    public required double Value { get; init; }
    public required IReadOnlyList<string> RocketsNames { get; init; }
}
