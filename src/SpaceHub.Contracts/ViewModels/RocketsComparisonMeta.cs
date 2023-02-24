namespace SpaceHub.Contracts.ViewModels;

public record RocketsComparisonMetaVM
{
    public required int TotalCount { get; init; }
    public required IReadOnlyDictionary<string, int> FamilyRocketsCountByName { get; init; }
    public required IReadOnlyDictionary<string, int> RocketIdsByName { get; init; }
}
