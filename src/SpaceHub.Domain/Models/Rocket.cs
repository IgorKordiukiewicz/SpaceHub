using SpaceHub.Domain.Attributes;

namespace SpaceHub.Domain.Models;

public class Rocket
{
    public required int ApiId { get; init; }
    public required string Name { get; init; }
    public required string Family { get; init; }
    public required string Variant { get; init; }
    public required bool Active { get; set; }
    public required bool Reusable { get; init; }
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public string WikiUrl { get; init; } = string.Empty;
    public string InfoUrl { get; init; } = string.Empty;

    [Symbol("m")] // TODO: Rename symbol attribute to Unit
    public double? Length { get; init; }

    [Symbol("m")]
    public double? Diameter { get; init; }

    public int? MaxStages { get; init; }

    [Symbol("$")]
    public long? LaunchCost { get; init; }

    [Symbol("T")]
    public int? LiftoffMass { get; init; }

    [Symbol("kN")]
    public int? ThrustAtLiftoff { get; init; }

    [Symbol("kg")]
    public int? LeoCapacity { get; init; }

    [Symbol("kg")]
    public int? GeoCapacity { get; init; }

    [Symbol("$")]
    public int? CostPerKgToLeo => GetCostPerKgToOrbit(LaunchCost, LeoCapacity);

    [Symbol("$")]
    public int? CostPerKgToGeo => GetCostPerKgToOrbit(LaunchCost, GeoCapacity);

    public int SuccessfulLaunches { get; set; }
    public int TotalLaunches { get; set; }
    public int ConsecutiveSuccessfulLaunches { get; set; }
    public int PendingLaunches { get; set; }

    [Symbol("%")]
    public int LaunchSuccess => TotalLaunches > 0 ? (int)Math.Round((double)SuccessfulLaunches * 100 / TotalLaunches) : 0;

    public DateTime? FirstFlight { get; init; }

    private static int? GetCostPerKgToOrbit(long? launchCost, int? orbitCapacity)
        => (launchCost is not null && orbitCapacity is not null && orbitCapacity > 0) ? (int)(launchCost.Value / orbitCapacity.Value) : null;
}
