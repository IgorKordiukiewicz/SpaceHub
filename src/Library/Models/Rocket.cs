using Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public record Rocket
    { 
        public int ApiId { get; init; }
        public string Name { get; init; }
        public string Family { get; init; }
        public string FullName { get; init; }
        public string Variant { get; init; }
        public record Detail
        {
            public string Description { get; init; }
            public Agency Manufacturer { get; init; }
            public string Alias { get; init; }
            public int? MinStages { get; init; }
            public int? MaxStages { get; init; }
            public double? Length { get; init; }
            public double? Diameter { get; init; }
            public DateTime? FirstFlight { get; init; }
            public int? LaunchCost { get; init; }
            public int? LiftoffMass { get; init; }
            public int? LeoCapacity { get; init; }
            public int? GeoCapacity { get; init; }
            public int? LiftoffThrust { get; init; }
            public int? Apogee { get; init; }
            public string? ImageUrl { get; init; }
            public string? WikiUrl { get; init; }
            public int TotalLaunchCount { get; init; }
            public int ConsecutiveSuccessfulLaunches { get; init; }
            public int SuccessfulLaunches { get; init; }
            public int FailedLaunches { get; init; }
            public int PendingLaunches { get; init; }
            public int LaunchSuccessPercent { get => (TotalLaunchCount > 0 ? (int)Math.Round((double)SuccessfulLaunches * 100 / TotalLaunchCount) : 0); }
            public int? CostPerKgToLeo { get => (LaunchCost != null && LeoCapacity != null && LeoCapacity > 0) ? LaunchCost / LeoCapacity : null; }
            public int? CostPerKgToGeo { get => (LaunchCost != null && GeoCapacity != null && GeoCapacity > 0) ? LaunchCost / GeoCapacity : null; }
            public Dictionary<RocketRankedPropertyType, int?>? RankedProperties { get; set; }
        }
        public Detail? Details { get; set; }
    }
}
