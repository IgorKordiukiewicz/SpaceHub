using Library.Api.Responses;

namespace Web.ViewModels
{
    public record RocketCardViewModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string? ImageUrl { get; init; }
        public string Family { get; init; }
        public string Variant { get; init; }
        public string Length { get; init; }
        public string Diameter { get; init; }
        public string MaxStages { get; init; }
        public string LaunchCost { get; init; }
        public string LiftoffMass { get; init; }
        public string LiftoffThrust { get; init; }
        public string GeoCapacity { get; init; }
        public string LeoCapacity { get; init; }
        public string FirstLaunch { get; init; }
        public string SuccessfulLaunches { get; init; }
        public string CostPerKgToLeo { get; init; }
        public string CostPerKgToGeo { get; init; }
    }
}
