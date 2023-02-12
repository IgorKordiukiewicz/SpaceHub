namespace SpaceHub.Contracts.ViewModels;

public record RocketsComparisonMetaVM
{
    public int TotalCount { get; init; }
    public Dictionary<string, int> FamilyGroupsWithItemsCount { get; init; } = new();
    public Dictionary<string, int> RocketNamesWithId { get; init;} = new();
}
