using Library.Api.Responses;

namespace Web.ViewModels
{
    public record RocketListItemViewModel
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Family { get; init; }
        public string Variant { get; init; }

        public RocketListItemViewModel(RocketConfigResponse rocket)
        {
            Id = rocket.Id;
            Name = rocket.Name;
            Family = rocket.Family;
            Variant = rocket.Variant;
        }
    }
}
