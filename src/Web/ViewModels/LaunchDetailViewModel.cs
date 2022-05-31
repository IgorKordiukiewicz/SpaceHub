using Library.Api.Responses;
using System.Globalization;

namespace Web.ViewModels
{
    public record LaunchDetailViewModel
    {
        public string Name { get; init; }
        public string ImageUrl { get; init; }
        public string Date { get; init; }
        public string? WindowStart { get; init; }
        public string? WindowEnd { get; init; }
        public string StatusName { get; init; }
        public string StatusDescription { get; init; }

        public LaunchDetailViewModel(LaunchDetailResponse launchResponse)
        {
            Name = launchResponse.Name;
            ImageUrl = launchResponse.ImageUrl;
            StatusName = launchResponse.Status.Name;
            StatusDescription = launchResponse.Status.Description;

            if(launchResponse.Date != null)
            {
                var date = launchResponse.Date.Value;
                if (date.Hour == 0 && date.Minute == 0)
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

            if (launchResponse.WindowStart != null)
            {
                var date = launchResponse.WindowStart.Value;
                if (date.Hour == 0 && date.Minute == 0)
                {
                    WindowStart = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    WindowStart = date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
            }

            if (launchResponse.WindowEnd != null)
            {
                var date = launchResponse.WindowStart.Value;
                if (date.Hour == 0 && date.Minute == 0)
                {
                    WindowEnd = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                {
                    WindowEnd = date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                }
            }
        }
    }
}
