using Library.Api.Responses;

namespace Web.ViewModels
{
    public class LaunchDetailViewModel
    {
        public string Name { get; set; }

        public LaunchDetailViewModel(LaunchDetailResponse launchResponse)
        {
            Name = launchResponse.Name;
        }
    }
}
