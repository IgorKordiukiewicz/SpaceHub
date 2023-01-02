using SpaceHub.Application.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SpaceHub.Application.Enums;

public enum ERocketProperty
{
    [Symbol("m")]
    Length,

    [Symbol("m")]
    Diameter,

    [Display(Name = "Max Stages")]
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
