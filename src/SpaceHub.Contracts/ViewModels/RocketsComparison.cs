using SpaceHub.Contracts.Enums;

namespace SpaceHub.Contracts.ViewModels;

public record RocketsComparisonVM
{
    public required IReadOnlyDictionary<Guid, IReadOnlyDictionary<ERocketComparisonProperty, RocketComparisonDatasetVM>> DatasetsById { get; init; }
}

public record RocketComparisonDatasetVM
{
    public double? Value { get; init; }
    public required double Fraction { get; init; }
    public double? Rank { get; init; }
}

