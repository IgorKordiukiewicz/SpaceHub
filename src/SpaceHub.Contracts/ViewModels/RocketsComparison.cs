using SpaceHub.Contracts.Enums;

namespace SpaceHub.Contracts.ViewModels;

public record RocketsComparisonVM
{
    public required IReadOnlyDictionary<Guid, IReadOnlyDictionary<ERocketComparisonProperty, double>> DatasetsById { get; init; }
}

