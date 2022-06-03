using Library.Api.Responses;
using System.Globalization;

namespace Web.ViewModels
{
    public record LaunchViewModel
    {
        public string Name { get; init; }
        public string ImageUrl { get; init; }    
        public string? Mission { get; init; }
        public string Date { get; init; }
        public string? WindowStart { get; init; }
        public string? WindowEnd { get; init; }
        public string StatusName { get; init; }
        public string StatusDescription { get; init; }
        public AgencyCardViewModel Agency { get; init; }
        public RocketCardViewModel Rocket { get; init; }
        public PadCardViewModel Pad { get; init; }

        public LaunchViewModel(LaunchDetailResponse launch)
        {
            Name = launch.Name;
            ImageUrl = launch.ImageUrl;
            Mission = launch.Mission?.Description;
            StatusName = launch.Status.Name;
            StatusDescription = launch.Status.Description;
            Date = Utils.DateToString(launch.Date) ?? string.Empty;
            WindowStart = Utils.DateToString(launch.WindowStart);
            WindowEnd = Utils.DateToString(launch.WindowEnd);

            Agency = new(launch.ServiceProvider);
            Rocket = new(launch.Rocket.Configuration);
            Pad = new(launch.Pad);
        }
    }
}
