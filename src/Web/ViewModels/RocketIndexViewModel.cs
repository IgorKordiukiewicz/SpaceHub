using Library.Api.Responses;

namespace Web.ViewModels
{
    public record RocketIndexViewModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Family { get; init; }
        public string Variant { get; init; }

        public RocketIndexViewModel(RocketConfigResponse rocketConfigResponse)
        {
            Id = rocketConfigResponse.Id;
            Name = rocketConfigResponse.Name;
            Family = rocketConfigResponse.Family;
            Variant = rocketConfigResponse.Variant;
        }
    }
}
