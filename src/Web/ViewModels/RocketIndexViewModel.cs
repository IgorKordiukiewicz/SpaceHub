using Library.Api.Responses;

namespace Web.ViewModels
{
    public record RocketIndexViewModel
    {
        public string Name { get; init; }
        public string Url { get; init; }
        public string Family { get; init; }
        public string Variant { get; init; }

        public RocketIndexViewModel(RocketConfigResponse rocketConfigResponse)
        {
            Name = rocketConfigResponse.Name;
            Url = rocketConfigResponse.Url;
            Family = rocketConfigResponse.Family;
            Variant = rocketConfigResponse.Variant;
        }
    }
}
