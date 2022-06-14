using Library.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Enums
{
    public enum RocketRankedPropertyType
    {
        [Display(Name = "Length")]
        [Symbol("m")]
        Length,

        [Display(Name = "Diameter")]
        [Symbol("m")]
        Diameter,

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
        SuccessfulLaunches,

        [Display(Name = "Total Launches")]
        TotalLaunches,

        [Display(Name = "Launch Success")]
        [Symbol("%")]
        LaunchSuccessPercent
    }
}
