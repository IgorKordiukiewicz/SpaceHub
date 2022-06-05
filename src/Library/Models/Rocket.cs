﻿using System;
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
            public int? MinStage { get; init; }
            public int? MaxStage { get; init; }
            public double? Length { get; init; }
            public double? Diameter { get; init; }
            public DateTime? FirstFlight { get; init; }
            public string? LaunchCost { get; init; }
            public int? LaunchMass { get; init; }
            public int? LeoCapacity { get; init; }
            public int? GeoCapacity { get; init; }
            public int? ThrustAtLiftoff { get; init; }
            public int? Apogee { get; init; }
            public string? ImageUrl { get; init; }
            public string? WikiUrl { get; init; }
            public int TotalLaunchCount { get; init; }
            public int ConsecutiveSuccessfulLaunches { get; init; }
            public int SuccessfulLaunches { get; init; }
            public int FailedLaunches { get; init; }
            public int PendingLaunches { get; init; }
        }
        public Detail? Details { get; set; }
    }
}
