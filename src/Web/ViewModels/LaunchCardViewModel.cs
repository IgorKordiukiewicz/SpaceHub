using Library.Api.Responses;
using System.Globalization;

namespace Web.ViewModels
{
    public record LaunchCardViewModel
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string ServiceProviderName { get; init; }
        public string PadName { get; init; }
        public string Status { get; init; }
        public string? Date { get; init; }

        public LaunchCardViewModel(LaunchResponse launch)
        {
            Id = launch.Id;
            Name = launch.Name;
            ImageUrl = launch.ImageUrl;
            ServiceProviderName = launch.ServiceProvider.Name;
            PadName = launch.Pad.Name;
            Status = launch.Status.Name;
            Date = Utils.DateToString(launch.Date);
        }
    }
}
