using SpaceHub.Contracts.Enums;

namespace SpaceHub.Contracts.ViewModels;

public record RocketsComparisonVM
{
    public required IReadOnlyDictionary<Guid, IReadOnlyDictionary<ERocketComparisonProperty, double>> ComparisonGroupsData { get; init; }
}

public record RocketPropertiesFractionsVM
{
    public required double Length { get; init; }
    public required double Diameter { get; init; }
    public required double LaunchCost { get; init; }
    public required double LiftoffMass { get; init; }
    public required double LiftoffThrust { get; init; }
    public required double LeoCapacity { get; init; }
    public required double GeoCapacity { get; init; }
    public required double SuccessfulLaunches { get; init; }
    public required double LaunchSuccess { get; init; }
    // TODO: Which properties to display
    // Only display CostPerKgToLEO/GEO instead of LEO/GEO Capacity + Launch cost?
}

