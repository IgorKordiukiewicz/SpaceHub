namespace SpaceHub.Contracts.ViewModels;

public record RocketsComparisonVM
{
    public Dictionary<Guid, RocketPropertiesFractionsVM> ComparisonGroupsData { get; init; } = new();
}

public record RocketPropertiesFractionsVM // TODO: Change name?
{
    // TODO: Add Range attribute? (0.0 - 1.0)
    public double Length { get; init; }
    public double Diameter { get; init; }
    public double LaunchCost { get; init; }
    public double LiftoffMass { get; init; }
    public double LiftoffThrust { get; init; }
    public double LeoCapacity { get; init; }
    public double GeoCapacity { get; init; }
    public double SuccessfulLaunches { get; init; }
    public double LaunchSuccess { get; init; }
    // TODO: Which properties to display
    // Only display CostPerKgToLEO/GEO instead of LEO/GEO Capacity + Launch cost?
}

