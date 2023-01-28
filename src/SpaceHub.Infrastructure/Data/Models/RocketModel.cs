using MongoDB.Bson.Serialization.Attributes;
using SpaceHub.Domain.Attributes;
using SpaceHub.Domain.Enums;

namespace SpaceHub.Infrastructure.Data.Models;

public class RocketModel
{
    [BsonId]
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
    public double? Length { get; init; }
    public double? Diameter { get; init; }
    public int? MaxStages { get; init; }
    public long? LaunchCost { get; init; }
    public int? LiftoffMass { get; init; }
    public int? ThrustAtLiftoff { get; init; }
    public int? LeoCapacity { get; init; }
    public int? GeoCapacity { get; init; }
    public int SuccessfulLaunches { get; set; }
    public int TotalLaunches { get; set; }
    public int ConsecutiveSuccessfulLaunches { get; set; }
    public int PendingLaunches { get; set; }
    public DateTime? FirstFlight { get; init; }
}
