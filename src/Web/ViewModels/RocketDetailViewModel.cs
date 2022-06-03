using Library.Api.Responses;

namespace Web.ViewModels
{
    public record RocketDetailViewModel
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

        public RocketDetailViewModel(RocketConfigDetailResponse configResponse)
        {
            Name = configResponse.Name;
            Description = configResponse.Description;
            ImageUrl = configResponse.ImageUrl;
            Length = Utils.ValueToStringWithSymbol(configResponse.Length, "m");
            Diameter = Utils.ValueToStringWithSymbol(configResponse.Diameter, "m");
            MaxStages = Utils.ValueToStringWithSymbol(configResponse.MaxStage, "");
            LaunchCost = Utils.ValueToStringWithSymbol(configResponse.LaunchCost, "$");
            LiftoffMass = Utils.ValueToStringWithSymbol(configResponse.LaunchMass, "T");
            LiftoffThrust = Utils.ValueToStringWithSymbol(configResponse.ThrustAtLiftoff, "kN");
            GeoCapacity = Utils.ValueToStringWithSymbol(configResponse.GeoCapacity, "kg");
            LeoCapacity = Utils.ValueToStringWithSymbol(configResponse.LeoCapacity, "kg");
            FirstLaunch = Utils.DateToString(configResponse.FirstFlight) ?? "-";

            var totalLaunches = configResponse.TotalLaunchCount;
            if (totalLaunches > 0)
            {
                var successPercent = (configResponse.SuccessfulLaunches * 100) / totalLaunches;
                SuccessfulLaunches = configResponse.SuccessfulLaunches.ToString() + "/" + totalLaunches.ToString()
                    + " (" + successPercent + "%)";
            }
            else
            {
                SuccessfulLaunches = "0/0";
            }
        }
    }
}
