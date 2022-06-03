using Library.Api.Responses;

namespace Web.ViewModels
{
    public record RocketCardViewModel
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string? ImageUrl { get; init; }
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

        public RocketCardViewModel(RocketConfigDetailResponse rocket)
        {
            Name = rocket.Name;
            Description = rocket.Description;
            ImageUrl = rocket.ImageUrl;
            Length = Utils.ValueToStringWithSymbol(rocket.Length, "m");
            Diameter = Utils.ValueToStringWithSymbol(rocket.Diameter, "m");
            MaxStages = Utils.ValueToStringWithSymbol(rocket.MaxStage, "");
            LaunchCost = Utils.ValueToStringWithSymbol(rocket.LaunchCost, "$");
            LiftoffMass = Utils.ValueToStringWithSymbol(rocket.LaunchMass, "T");
            LiftoffThrust = Utils.ValueToStringWithSymbol(rocket.ThrustAtLiftoff, "kN");
            GeoCapacity = Utils.ValueToStringWithSymbol(rocket.GeoCapacity, "kg");
            LeoCapacity = Utils.ValueToStringWithSymbol(rocket.LeoCapacity, "kg");
            FirstLaunch = Utils.DateToString(rocket.FirstFlight) ?? "-";

            var totalLaunches = rocket.TotalLaunchCount;
            if (totalLaunches > 0)
            {
                var successPercent = (rocket.SuccessfulLaunches * 100) / totalLaunches;
                SuccessfulLaunches = rocket.SuccessfulLaunches.ToString() + "/" + totalLaunches.ToString()
                    + " (" + successPercent + "%)";
            }
            else
            {
                SuccessfulLaunches = "0/0";
            }
        }
    }
}
