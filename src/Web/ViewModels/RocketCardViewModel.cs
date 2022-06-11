using Library.Api.Responses;
using Library.Enums;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public record RocketCardViewModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string? ImageUrl { get; init; }
        public string? InfoUrl { get; init; }
        public string? WikiUrl { get; init; }
        public string Family { get; init; }
        public string Variant { get; init; }
        public string Length { get; init; }
        public string Diameter { get; init; }

        [Display(Name = "Max Stages")]
        public string MaxStages { get; init; }

        [Display(Name = "Launch Cost")]
        public string LaunchCost { get; init; }

        [Display(Name = "Liftoff Mass")]
        public string LiftoffMass { get; init; }

        [Display(Name = "Liftoff Thrust")]
        public string LiftoffThrust { get; init; }

        [Display(Name = "GEO Capacity")]
        public string GeoCapacity { get; init; }

        [Display(Name = "LEO Capacity")]
        public string LeoCapacity { get; init; }

        [Display(Name = "Successful Launches")]
        public string SuccessfulLaunches { get; init; }

        [Display(Name = "Total Launches")]
        public string TotalLaunches { get; init; }

        [Display(Name = "First Launch")]
        public string FirstLaunch { get; init; }

        [Display(Name = "Launch Success")]
        public string LaunchSuccessPercent { get; init; }

        [Display(Name = "Cost per kg to LEO")]
        public string CostPerKgToLeo { get; init; }

        [Display(Name = "Cost per kg to GEO")]
        public string CostPerKgToGeo { get; init; }

        public Dictionary<RocketRankedPropertyType, int?> RankedProperties { get; init; }
    }
}
