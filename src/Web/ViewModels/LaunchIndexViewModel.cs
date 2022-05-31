using Library.Api.Responses;
using System.Globalization;

namespace Web.ViewModels
{
    public record LaunchIndexViewModel
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string ServiceProviderName { get; init; }
        public string PadName { get; init; }
        public string Status { get; init; }
        public string Date { get; init; }

        public LaunchIndexViewModel(LaunchResponse launchResponse)
        {
            Id = launchResponse.Id;
            Name = launchResponse.Name;
            ImageUrl = launchResponse.ImageUrl;
            ServiceProviderName = launchResponse.ServiceProvider.Name;
            PadName = launchResponse.Pad.Name;
            Status = launchResponse.Status.Name;

            if(launchResponse.Date != null)
            {
                var date = launchResponse.Date.Value;
                if(date.Hour == 0 && date.Minute == 0)
                {
                    Date = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    Date = date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
            }
            else
            {
                Date = string.Empty;
            }
        }
    }
}
