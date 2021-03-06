using Library.Api.Responses;

namespace Web.ViewModels
{
    public record PadCardViewModel
    {
        public string Name { get; init; }
        public string LocationName { get; init; }
        public string MapImageUrl { get; init; }
        public string? InfoUrl { get; init; }
        public string? WikiUrl { get; init; }
    }
}
