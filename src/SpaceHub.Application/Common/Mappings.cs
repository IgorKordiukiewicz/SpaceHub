using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Application.Common;

public static class Mappings
{
    public static Rocket ToDomainModel(this RocketModel rocket)
    {
        return new()
        {
            ApiId = rocket.ApiId,
            Name = rocket.Name,
            Family = rocket.Family,
            Variant = rocket.Variant,
            Active = rocket.Active,
            Reusable = rocket.Reusable,
            Description = rocket.Description,
            ImageUrl = rocket.ImageUrl,
            WikiUrl = rocket.WikiUrl,
            InfoUrl = rocket.InfoUrl,
            Length = rocket.Length,
            Diameter = rocket.Diameter,
            MaxStages = rocket.MaxStages,
            LaunchCost = rocket.LaunchCost,
            LiftoffMass = rocket.LiftoffMass,
            LiftoffThrust = rocket.ThrustAtLiftoff,
            LeoCapacity = rocket.LeoCapacity,
            GeoCapacity = rocket.GeoCapacity,
            SuccessfulLaunches = rocket.SuccessfulLaunches,
            TotalLaunches = rocket.TotalLaunches,
            ConsecutiveSuccessfulLaunches = rocket.ConsecutiveSuccessfulLaunches,
            PendingLaunches = rocket.PendingLaunches,
            FirstFlight = rocket.FirstFlight
        };
    }

    public static RocketVM ToViewModel(this Rocket rocket)
    {
        return new()
        {
            ApiId = rocket.ApiId,
            Name = rocket.Name,
            Description = rocket.Description,
            ImageUrl = rocket.ImageUrl,
            Length = rocket.Length,
            Diameter = rocket.Diameter,
            MaxStages = rocket.MaxStages,
            LaunchCost = rocket.LaunchCost,
            LiftoffMass = rocket.LiftoffMass,
            LiftoffThrust = rocket.LiftoffThrust,
            LeoCapacity = rocket.LeoCapacity,
            GeoCapacity = rocket.GeoCapacity,
            CostPerKgToLeo = rocket.CostPerKgToLeo,
            CostPerKgToGeo = rocket.CostPerKgToGeo,
            SuccessfulLaunches = rocket.SuccessfulLaunches,
            TotalLaunches = rocket.TotalLaunches,
            LaunchSuccess = rocket.LaunchSuccess,
            FirstFlight = rocket.FirstFlight is not null ? DateOnly.FromDateTime(rocket.FirstFlight.Value) : null,
        };
    }
}
