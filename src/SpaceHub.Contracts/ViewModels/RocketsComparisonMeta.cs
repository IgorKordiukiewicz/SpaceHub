namespace SpaceHub.Contracts.ViewModels;

public record RocketsComparisonMetaVM
{
    public int TotalCount { get; init; }
    public Dictionary<string, int> FamilyRocketsCountByName { get; init; } = new();
    public Dictionary<string, int> RocketIdsByName { get; init;} = new();
}
