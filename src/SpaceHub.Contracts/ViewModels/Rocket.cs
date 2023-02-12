namespace SpaceHub.Contracts.ViewModels;

public record RocketVM
{
    public required int ApiId { get; init; }
    public required string Name { get; init; }
    public string Family { get; init; } = string.Empty;
    public string Variant { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public double? Length { get; init; }
    public double? Diameter { get; init; }
    public int? MaxStages { get; init; }
    public long? LaunchCost { get; init; }
    public int? LiftoffMass { get; init; }
    public int? LiftoffThrust { get; init; }
    public int? LeoCapacity { get; init; }
    public int? GeoCapacity { get; init; }
    public int? CostPerKgToLeo { get; init; }
    public int? CostPerKgToGeo { get; init; }
    public int? SuccessfulLaunches { get; init; }
    public int? TotalLaunches { get; init; }
    public int? LaunchSuccess { get; init; }
    public DateOnly? FirstFlight { get; init; }
}