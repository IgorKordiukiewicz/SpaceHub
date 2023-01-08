using SpaceHub.Domain.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SpaceHub.Domain.Enums;

public enum ERocketProperty
{
    [Symbol("m")]
    Length,

    [Symbol("m")] // TODO: maybe instead define another enum called EUnit -> Meter, Kilogram, Ton, kN, etc. and they will also have display attrbitues
    Diameter,

    [Display(Name = "Max Stages")] // TODO: Should display attributes be in domain layer?
    MaxStages,

    [Display(Name = "Launch Cost")]
    [Symbol("$")]
    LaunchCost,

    [Display(Name = "Liftoff Mass")]
    [Symbol("T")]
    LiftoffMass,

    [Display(Name = "Liftoff Thrust")]
    [Symbol("kN")]
    LiftoffThrust,

    [Display(Name = "LEO Capacity")]
    [Symbol("kg")]
    LeoCapacity,

    [Display(Name = "GEO Capacity")]
    [Symbol("kg")]
    GeoCapacity,

    [Display(Name = "Cost per kg to LEO")]
    [Symbol("$")]
    CostPerKgToLeo,

    [Display(Name = "Cost per kg to GEO")]
    [Symbol("$")]
    CostPerKgToGeo,

    [Display(Name = "Successful Launches")]
    SuccessfullLaunches,

    [Display(Name = "Total Launches")]
    TotalLaunches,

    [Display(Name = "Launch Success")]
    [Symbol("%")]
    LaunchSuccessPercent,

    [Display(Name = "First Flight")]
    FirstFlight
}
