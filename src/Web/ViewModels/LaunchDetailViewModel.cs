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
        public RocketDetailViewModel Rocket { get; init; }
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

            Rocket = new(launchResponse.Rocket.Configuration);

            PadName = launchResponse.Pad.Name;
            PadLocationName = launchResponse.Pad.Location.Name;
            PadMapImageUrl = launchResponse.Pad.MapImageUrl;
        }
    }
}
