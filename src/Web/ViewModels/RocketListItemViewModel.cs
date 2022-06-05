using Library.Api.Responses;

namespace Web.ViewModels
{
    public record RocketListItemViewModel
    {
        public int ApiId { get; init; }
        public string Name { get; init; }
        public string Family { get; init; }
        public string Variant { get; init; }
    }
}
