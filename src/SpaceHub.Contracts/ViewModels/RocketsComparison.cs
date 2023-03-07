using SpaceHub.Contracts.Enums;

namespace SpaceHub.Contracts.ViewModels;

public record RocketsComparisonVM
{
    public required IReadOnlyDictionary<Guid, IReadOnlyDictionary<ERocketComparisonProperty, RocketComparisonDatasetVM>> DatasetsById { get; init; }
}

// TODO: Consider removing datasets types and only compare individual rockets -> change Rank to int
public record RocketComparisonDatasetVM
{
    public required double? Value { get; init; }
    public required double Fraction { get; init; }
    public required double? Rank { get; init; }
}

