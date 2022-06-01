using Library.Api.Responses;
using System.Globalization;

namespace Web.ViewModels
{
    public record LaunchDetailViewModel
    {
        public string Name { get; init; }
        public string ImageUrl { get; init; }    
        public string? Mission { get; init; }
        public string Date { get; init; }
        public string? WindowStart { get; init; }
        public string? WindowEnd { get; init; }
        public string StatusName { get; init; }
        public string StatusDescription { get; init; }
        public string AgencyName { get; init; }
        public string AgencyCountryCode { get; init; }
        public string? AgencyImageUrl { get; init; }
        public string? AgencyDescription { get; init; }
        public string RocketName { get; init; }
        public string RocketDescription { get; init; }
        public string? RocketImageUrl { get; init; }
        public string RocketLength { get; init; }
        public string RocketDiameter { get; init; }
        public string RocketMaxStages { get; init; }
        public string RocketLaunchCost { get; init; }
        public string RocketLiftoffMass { get; init; }
        public string RocketLiftoffThrust { get; init; }
        public string RocketGeoCapacity { get; init; }
        public string RocketLeoCapacity { get; init; }
        public string RocketFirstLaunch { get; init; }
        public string RocketSuccessfulLaunches { get; init; }
        public string PadName { get; init; }
        public string PadLocationName { get; init; }
        public string PadMapImageUrl { get; init; }

        public LaunchDetailViewModel(LaunchDetailResponse launchResponse)
        {
            Name = launchResponse.Name;
            ImageUrl = launchResponse.ImageUrl;
            Mission = launchResponse.Mission?.Description;
            StatusName = launchResponse.Status.Name;
            StatusDescription = launchResponse.Status.Description;
            Date = Utils.DateToString(launchResponse.Date) ?? string.Empty;
            WindowStart = Utils.DateToString(launchResponse.WindowStart);
            WindowEnd = Utils.DateToString(launchResponse.WindowEnd);

            AgencyName = launchResponse.ServiceProvider.Name;
            AgencyCountryCode = launchResponse.ServiceProvider.CountryCode;
            AgencyImageUrl = launchResponse.ServiceProvider.LogoUrl;
            AgencyDescription = launchResponse.ServiceProvider.Description;

            RocketName = launchResponse.Rocket.Configuration.Name;
            RocketDescription = launchResponse.Rocket.Configuration.Description;
            RocketImageUrl = launchResponse.Rocket.Configuration.ImageUrl;
            RocketLength = Utils.ValueToStringWithSymbol(launchResponse.Rocket.Configuration.Length, "m");
            RocketDiameter = Utils.ValueToStringWithSymbol(launchResponse.Rocket.Configuration.Diameter, "m");
            RocketMaxStages = Utils.ValueToStringWithSymbol(launchResponse.Rocket.Configuration.MaxStage, "");
            RocketLaunchCost = Utils.ValueToStringWithSymbol(launchResponse.Rocket.Configuration.LaunchCost, "$");
            RocketLiftoffMass = Utils.ValueToStringWithSymbol(launchResponse.Rocket.Configuration.LaunchMass, "T");
            RocketLiftoffThrust = Utils.ValueToStringWithSymbol(launchResponse.Rocket.Configuration.ThrustAtLiftoff, "kN");
            RocketGeoCapacity = Utils.ValueToStringWithSymbol(launchResponse.Rocket.Configuration.GeoCapacity, "kg");
            RocketLeoCapacity = Utils.ValueToStringWithSymbol(launchResponse.Rocket.Configuration.LeoCapacity, "kg");
            RocketFirstLaunch = Utils.DateToString(launchResponse.Rocket.Configuration.FirstFlight) ?? "-";

            var totalLaunches = launchResponse.Rocket.Configuration.TotalLaunchCount;
            if(totalLaunches > 0)
            {
                var successPercent = (launchResponse.Rocket.Configuration.SuccessfulLaunches * 100) / totalLaunches;
                RocketSuccessfulLaunches = launchResponse.Rocket.Configuration.SuccessfulLaunches.ToString() + "/" + totalLaunches.ToString()
                    + " (" + successPercent + "%)";
            }
            else
            {
                RocketSuccessfulLaunches = "0/0";
            }

            PadName = launchResponse.Pad.Name;
            PadLocationName = launchResponse.Pad.Location.Name;
            PadMapImageUrl = launchResponse.Pad.MapImageUrl;
        }
    }
}
